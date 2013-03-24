﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.UI;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MyToolkit.Utilities
{
	public static class PageUtilities
	{
		/// <summary>
		/// Call this method in Loaded event as the event will be automatically deregistred when the FrameworkElement has been unloaded
		/// </summary>
		public static void RegisterBackKey(Page page)
		{
			var callback = new TypedEventHandler<CoreDispatcher, AcceleratorKeyEventArgs>(
				delegate(CoreDispatcher sender, AcceleratorKeyEventArgs args)
				{
					if (!args.Handled && args.VirtualKey == VirtualKey.Back &&
						(args.EventType == CoreAcceleratorKeyEventType.KeyDown || 
							args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown))
					{
						var element = FocusManager.GetFocusedElement();
						if (element is FrameworkElement && PopupHelper.IsInPopup((FrameworkElement)element))
							return;

						if (element is TextBox || element is PasswordBox || element is WebView)
							return; 

						if (page.Frame.CanGoBack)
						{
							args.Handled = true;
							page.Frame.GoBack();
						}
					}
				});

			page.Dispatcher.AcceleratorKeyActivated += callback;

			SingleEvent.Register(page,
				(p, h) => p.Unloaded += h,
				(p, h) => p.Unloaded -= h,
				(o, a) => { page.Dispatcher.AcceleratorKeyActivated -= callback; });
		}

		/// <summary>
		/// Call this method in Loaded event as the event will be automatically deregistred when the FrameworkElement has been unloaded
		/// </summary>
		/// <param name="page"></param>
		/// <param name="handler"></param>
		public static void RegisterAcceleratorKeyActivated(FrameworkElement page, TypedEventHandler<CoreDispatcher, AcceleratorKeyEventArgs> handler)
		{
			page.Dispatcher.AcceleratorKeyActivated += handler;
			SingleEvent.Register(page, (p, h) => p.Unloaded += h, (p, h) => p.Unloaded -= h, (o, a) =>
			{
				page.Dispatcher.AcceleratorKeyActivated -= handler;
			});
		}

		private class SearchKeyContainer
		{
			private readonly Action searchKeyPressed;
			public SearchKeyContainer(Action searchKeyPressed)
			{
				this.searchKeyPressed = searchKeyPressed;
			}

			private bool ctrlDown = false;
			public void AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
			{
				if (args.VirtualKey == VirtualKey.Control)
					ctrlDown = args.EventType == CoreAcceleratorKeyEventType.KeyDown;

				if (args.VirtualKey == VirtualKey.F3 || (args.VirtualKey == VirtualKey.F && ctrlDown))
				{
					searchKeyPressed();
					args.Handled = true;
				}
			}
		}

		/// <summary>
		/// Call this method in Loaded event as the event will be automatically deregistred when the FrameworkElement has been unloaded
		/// </summary>
		public static void RegisterSearchPressed(FrameworkElement page, Action searchKeyPressed)
		{
			var c = new SearchKeyContainer(searchKeyPressed);
			RegisterAcceleratorKeyActivated(page, c.AcceleratorKeyActivated);
		}
	}
}
