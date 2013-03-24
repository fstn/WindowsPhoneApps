﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using MyToolkit.Networking;

#if WINRT
using System.Threading;
using MyToolkit.Utilities;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Controls;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
#endif

namespace MyToolkit.Media
{
	public static class ImageHelper
	{
		public static bool IsUsed { get; set; }

		private const int WorkItemQuantum = 5;

		private static bool _exiting;
		private static readonly object mutex = new object();
		private static readonly Queue<PendingRequest> _pendingRequests = new Queue<PendingRequest>();
		private static readonly Queue<IAsyncResult> _pendingResponses = new Queue<IAsyncResult>();
		private static readonly object _syncBlock = new object();

		public static Uri GetSource(Image obj)
		{
			if (null == obj)
				throw new ArgumentNullException("obj");
			return (Uri)obj.GetValue(SourceProperty);
		}

		public static void SetSource(Image obj, Uri value)
		{
			if (null == obj)
				throw new ArgumentNullException("obj");
			obj.SetValue(SourceProperty, value);
		}

		public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
			"Source", typeof(Uri), typeof(ImageHelper), new PropertyMetadata(null, OnSourceChanged));

		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification = "Static constructor performs additional tasks.")]
		static ImageHelper()
		{
#if WINRT
			ThreadPool.RunAsync(WorkerThreadProc);
			Application.Current.Suspending += HandleApplicationExit;
#else
			ThreadPool.QueueUserWorkItem(WorkerThreadProc);
			Application.Current.Exit += HandleApplicationExit;
#endif
			IsUsed = true;
		}

#if WINRT
		private static void HandleApplicationExit(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
#else
		private static void HandleApplicationExit(object sender, EventArgs eventArgs)
#endif
		{
			_exiting = true;
			if (Monitor.TryEnter(_syncBlock, 100))
			{
				Monitor.Pulse(_syncBlock);
				Monitor.Exit(_syncBlock);
			}
		}

		private static bool _isEnabled = true;
		public static bool IsEnabled
		{
			get { lock (mutex) { return _isEnabled; } }
			set
			{
				lock (mutex)
				{
					if (_isEnabled == value)
						return;
					_isEnabled = value;
				}

				if (value) // is active
				{
					lock (_syncBlock)
						Monitor.Pulse(_syncBlock);
				}
			}
		} 

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Relevant exceptions don't have a common base class.")]
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Linear flow is easy to understand.")]
		private static void WorkerThreadProc(object unused)
		{
			var rand = new Random();
			var pendingRequests = new List<PendingRequest>();
			var pendingResponses = new Queue<IAsyncResult>();
			while (!_exiting)
			{
				lock (_syncBlock)
				{
					if (!IsEnabled || ((0 == _pendingRequests.Count) && (0 == _pendingResponses.Count) &&
						(0 == pendingRequests.Count) && (0 == pendingResponses.Count)))
					{
						Monitor.Wait(_syncBlock);
						if (_exiting)
							return;
					}

					if (!IsEnabled)
						continue;

					while (0 < _pendingRequests.Count)
					{
						var pendingRequest = _pendingRequests.Dequeue();
						for (var i = 0; i < pendingRequests.Count; i++)
						{
							if (pendingRequests[i].Image == pendingRequest.Image)
							{
								pendingRequests[i] = pendingRequest;
								pendingRequest = null;
								break;
							}
						}

						if (pendingRequest != null)
							pendingRequests.Add(pendingRequest);
					}

					while (0 < _pendingResponses.Count)
						pendingResponses.Enqueue(_pendingResponses.Dequeue());
				}

				var pendingCompletions = new Queue<PendingCompletion>();

				var count = pendingRequests.Count;
				for (var i = 0; (0 < count) && (i < WorkItemQuantum); i++)
				{
					var index = rand.Next(count);
					var pendingRequest = pendingRequests[index];
					pendingRequests[index] = pendingRequests[count - 1];
					pendingRequests.RemoveAt(count - 1);
					count--;

					if (pendingRequest.Uri == null)
						continue;

					if (pendingRequest.Uri.IsAbsoluteUri)
					{
						var webRequest = WebRequest.CreateHttp(pendingRequest.Uri);

						if (pendingRequest.Uri is AuthenticatedUri)
							webRequest.Credentials = ((AuthenticatedUri)pendingRequest.Uri).Credentials;

#if !WINRT
						webRequest.AllowReadStreamBuffering = true; // Don't want to block this thread or the UI thread on network access
#endif

						webRequest.BeginGetResponse(HandleGetResponseResult, new ResponseState(webRequest, pendingRequest.Image, pendingRequest.Uri));
					}
					else
					{
						var originalUriString = pendingRequest.Uri.OriginalString;
						var resourceStreamUri = originalUriString.StartsWith("/", StringComparison.Ordinal) ? new Uri(originalUriString.TrimStart('/'), UriKind.Relative) : pendingRequest.Uri;

#if WINRT
						try
						{
							var file = StorageFile.GetFileFromApplicationUriAsync(resourceStreamUri).RunSynchronouslyWithResult();
							var stream = file.OpenStreamForReadAsync().RunSynchronouslyWithResult();
							if (stream != null)
								pendingCompletions.Enqueue(new PendingCompletion(pendingRequest.Image, pendingRequest.Uri, null, stream));
						} catch { }
#else
						var streamResourceInfo = Application.GetResourceStream(resourceStreamUri);
						if (streamResourceInfo != null)
							pendingCompletions.Enqueue(new PendingCompletion(pendingRequest.Image, pendingRequest.Uri, null, streamResourceInfo.Stream));
#endif
					}

					Thread.Sleep(1);
				}

				for (var i = 0; (0 < pendingResponses.Count) && (i < WorkItemQuantum); i++)
				{
					var pendingResponse = pendingResponses.Dequeue();
					var responseState = (ResponseState)pendingResponse.AsyncState;
					try
					{
						var response = responseState.WebRequest.EndGetResponse(pendingResponse);
						pendingCompletions.Enqueue(new PendingCompletion(responseState.Image, responseState.Uri, response, response.GetResponseStream()));
					}
					catch (WebException)
					{
						pendingCompletions.Enqueue(new PendingCompletion(responseState.Image, responseState.Uri, null, null));
					}

					Thread.Sleep(1);
				}

				if (0 < pendingCompletions.Count)
				{
#if WINRT
					pendingCompletions.Peek().Image.Dispatcher.
						RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
#else
					Deployment.Current.Dispatcher.BeginInvoke(() =>
#endif
					{
						while (0 < pendingCompletions.Count)
						{
							using (var pendingCompletion = pendingCompletions.Dequeue())
							{
								var source = GetSource(pendingCompletion.Image);
								if (source != null && source.Equals(pendingCompletion.Uri))
								{
									var bitmap = new BitmapImage();
									if (pendingCompletion.Stream != null)
									{
										try
										{
#if WINRT
											var local = pendingCompletion;
											var newStream = pendingCompletion.Stream.AsRandomAccessStream();
											var task = bitmap.SetSourceAsync(newStream).AsTask();
											task.GetAwaiter().OnCompleted(delegate
												{
													newStream.Dispose();
													if (task.Exception != null)
														bitmap.UriSource = new Uri("http://0.0.0.0");
													local.Image.Source = bitmap;
													local.Dispose();
												});
#else
											bitmap.SetSource(pendingCompletion.Stream);
											pendingCompletion.Image.Source = bitmap;
											pendingCompletion.Dispose();
#endif
										}
										catch (Exception e)
										{ 
											bitmap.UriSource = new Uri("http://0.0.0.0");
											pendingCompletion.Image.Source = bitmap;
											pendingCompletion.Dispose();
										}
									}
									else // web exception
									{
										bitmap.UriSource = new Uri("http://0.0.0.0"); // TODO used to trigger ImageFailed => better way?
										pendingCompletion.Image.Source = bitmap;
										pendingCompletion.Dispose();
									}
								}
							}
						}
					});
				}
			}
		}

		private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var image = (Image)obj;
			var uri = (Uri)e.NewValue;

#if WINRT
        	if (!IsUsed ||	Windows.ApplicationModel.DesignMode.DesignModeEnabled)
#else
			if (!IsUsed || DesignerProperties.IsInDesignTool)
#endif
				image.Source = new BitmapImage(uri);
			else
			{
				// Clear-out the current image because it's now stale (helps when used with virtualization)
				image.Source = null;
				lock (_syncBlock)
				{
					_pendingRequests.Enqueue(new PendingRequest(image, uri));
					Monitor.Pulse(_syncBlock);
				}
			}
		}

		private static void HandleGetResponseResult(IAsyncResult result)
		{
			lock (_syncBlock)
			{
				_pendingResponses.Enqueue(result);
				Monitor.Pulse(_syncBlock);
			}
		}

		private class PendingRequest
		{
			public Image Image { get; private set; }
			public Uri Uri { get; private set; }
			public PendingRequest(Image image, Uri uri)
			{
				Image = image;
				Uri = uri;
			}
		}

		private class ResponseState
		{
			public WebRequest WebRequest { get; private set; }
			public Image Image { get; private set; }
			public Uri Uri { get; private set; }
			public ResponseState(WebRequest webRequest, Image image, Uri uri)
			{
				WebRequest = webRequest;
				Image = image;
				Uri = uri;
			}
		}

		private class PendingCompletion : IDisposable
		{
			public Image Image { get; private set; }
			public Uri Uri { get; private set; }
			public WebResponse Response { get; private set; }
			public Stream Stream { get; private set; }

			public PendingCompletion(Image image, Uri uri, WebResponse response, Stream stream)
			{
				Image = image;
				Uri = uri;
				Response = response;
				Stream = stream;
			}

			public void Dispose()
			{
				if (Response != null)
					Response.Dispose();
				Response = null;

				if (Stream != null)
					Stream.Dispose();
				Stream = null;
			}
		}
	}
}
