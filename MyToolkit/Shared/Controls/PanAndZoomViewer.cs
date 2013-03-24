﻿using System;

#if WINRT
using MyToolkit.Mathematics;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

#else
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
#endif

namespace MyToolkit.Controls
{
#if WINRT
	[ContentProperty(Name = "Content")]
#else
	[ContentProperty("Content")]
#endif
	public class PanAndZoomViewer : Control
	{
		public PanAndZoomViewer()
		{
			DefaultStyleKey = typeof(PanAndZoomViewer);
		}

		#region Dependency Properties
		
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof (FrameworkElement), typeof (PanAndZoomViewer), new PropertyMetadata(default(FrameworkElement)));

		public FrameworkElement Content
		{
			get { return (FrameworkElement) GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static readonly DependencyProperty MaxZoomFactorProperty =
			DependencyProperty.Register("MaxZoomFactor", typeof (double), typeof (PanAndZoomViewer), new PropertyMetadata(5.0));

		public double MaxZoomFactor
		{
			get { return (double) GetValue(MaxZoomFactorProperty); }
			set { SetValue(MaxZoomFactorProperty, value); }
		}
	
		#endregion

		private ContentPresenter content; 
		private RectangleGeometry geometry; 

#if WINRT
		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			
			ManipulationMode = ManipulationModes.All;
			PointerWheelChanged += OnPointerWheelChanged;
#else
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var listener = GestureService.GetGestureListener(this);
			listener.PinchStarted += OnPinchStarted;
			listener.PinchDelta += OnPinchDelta;
			listener.DragDelta += OnDragDelta;			
#endif

			content = (ContentPresenter)GetTemplateChild("content");
			if (content.Content == null)
				content.Content = Content;
			
			geometry = new RectangleGeometry();
			Clip = geometry;

			SizeChanged += delegate { UpdateGeometry(); };
			UpdateGeometry();
		}

		public void CalculateMaxZoomFactor(int maxWidth, int maxHeight)
		{
			var horizontalZoom = maxWidth / ActualWidth;
			var verticalZoom = maxHeight / ActualHeight;

			MaxZoomFactor = horizontalZoom > verticalZoom ? horizontalZoom : verticalZoom;
		}

#if WINRT
	
		private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs args)
		{
			var pt = args.GetCurrentPoint(this);
			var delta = pt.Properties.MouseWheelDelta / 120;
			var diff = MaxZoomFactor/20*delta;
			
			currentScale = currentScale + diff;
			if (currentScale < 1.0)
				currentScale = 1.0;
			if (currentScale > MaxZoomFactor)
				currentScale = MaxZoomFactor;

			currentPosition = CalculatePositionByPoint(pt.Position, currentScale);

			ApplyScaleAndPosition(false);

			args.Handled = true; 
		}


		protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
		{
			e.Handled = true;
			base.OnManipulationStarted(e);
		}

		protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
		{
			if (isAnimating)
				return;

			var scaleFactor = e.Delta.Scale;
			if (!IsScaleValid(scaleFactor))
				return;

			UpdatScale(scaleFactor);

			var translationDelta = e.Delta.Translation.
				Subtract(
					(e.Position.X * scaleFactor - e.Position.X) * currentScale,
					(e.Position.Y * scaleFactor - e.Position.Y) * currentScale);
			
			UpdatePosition(translationDelta);

			e.Handled = true; 
			base.OnManipulationDelta(e);
		}

		//private Point GetTranslationDelta(Point currentFinger1, Point currentFinger2, double scaleFactor)
		//{
		//	var newPos1 = new Point(
		//		currentFinger1.X + (currentPosition.X - oldFinger1.X) * scaleFactor,
		//		currentFinger1.Y + (currentPosition.Y - oldFinger1.Y) * scaleFactor);

		//	var newPos2 = new Point(
		//		currentFinger2.X + (currentPosition.X - oldFinger2.X) * scaleFactor,
		//		currentFinger2.Y + (currentPosition.Y - oldFinger2.Y) * scaleFactor);

		//	var newPos = new Point(
		//		(newPos1.X + newPos2.X) / 2,
		//		(newPos1.Y + newPos2.Y) / 2);

		//	return new Point(
		//		newPos.X - currentPosition.X,
		//		newPos.Y - currentPosition.Y);
		//}

		protected override void OnManipulationCompleted(Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
		{
			e.Handled = true;
			base.OnManipulationCompleted(e);
		}

#else
		
		protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
		{
			e.Handled = true;
			base.OnManipulationStarted(e);
		}

		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			e.Handled = true;
			base.OnManipulationDelta(e);
		}

		private Point oldFinger1;
		private Point oldFinger2;
		private double oldScaleFactor;

		private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
		{
			if (isAnimating)
				return; 

			oldFinger1 = e.GetPosition(content, 0);
			oldFinger2 = e.GetPosition(content, 1);
			oldScaleFactor = 1;
			e.Handled = true; 
		}

		private void OnPinchDelta(object sender, PinchGestureEventArgs e)
		{
			if (isAnimating)
				return; 

			var scaleFactor = e.DistanceRatio / oldScaleFactor;
			if (!IsScaleValid(scaleFactor))
				return;

			var currentFinger1 = e.GetPosition(content, 0);
			var currentFinger2 = e.GetPosition(content, 1);

			var translationDelta = GetTranslationDelta(currentFinger1, currentFinger2, scaleFactor);

			oldFinger1 = currentFinger1;
			oldFinger2 = currentFinger2;
			oldScaleFactor = e.DistanceRatio;

			UpdatScale(scaleFactor);
			UpdatePosition(translationDelta);

			e.Handled = true; 
		}

		private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
		{
			if (isAnimating)
				return; 

			var translationDelta = new Point(e.HorizontalChange, e.VerticalChange);
			if (IsDragValid(1, translationDelta))
				UpdatePosition(translationDelta);

			e.Handled = true; 
		}

		private bool IsDragValid(double scaleDelta, Point translateDelta)
		{
			if (currentPosition.X + translateDelta.X > 0 || currentPosition.Y + translateDelta.Y > 0)
				return false;

			if ((content.ActualWidth * currentScale * scaleDelta) + (currentPosition.X + translateDelta.X) < content.ActualWidth)
				return false;

			if ((content.ActualHeight * currentScale * scaleDelta) + (currentPosition.Y + translateDelta.Y) < content.ActualHeight)
				return false;

			return true;
		}

		private Point GetTranslationDelta(Point currentFinger1, Point currentFinger2, double scaleFactor)
		{
			var newPos1 = new Point(
				currentFinger1.X + (currentPosition.X - oldFinger1.X) * scaleFactor,
				currentFinger1.Y + (currentPosition.Y - oldFinger1.Y) * scaleFactor);

			var newPos2 = new Point(
				currentFinger2.X + (currentPosition.X - oldFinger2.X) * scaleFactor,
				currentFinger2.Y + (currentPosition.Y - oldFinger2.Y) * scaleFactor);

			var newPos = new Point(
				(newPos1.X + newPos2.X) / 2,
				(newPos1.Y + newPos2.Y) / 2);

			return new Point(
				newPos.X - currentPosition.X,
				newPos.Y - currentPosition.Y);
		}

#endif
		
		private bool isAnimating = false; 

#if WINRT
		protected override void OnDoubleTapped(Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
#else
		protected override void OnDoubleTap(System.Windows.Input.GestureEventArgs e)
#endif
		{
			if (isAnimating)
				return; 

			e.Handled = true;
			
			if (currentScale == 1 && currentPosition.X == 0 && currentPosition.Y == 0)
			{
				var point = e.GetPosition(this);
				currentScale = MaxZoomFactor;
				currentPosition = CalculatePositionByPoint(point, currentScale);
				ApplyScaleAndPosition(true);
			}
			else
				ResetZoomAndPosition(true);
	
#if WINRT
 			base.OnDoubleTapped(e);
#else
			base.OnDoubleTap(e);
#endif
		}

		private Point CalculatePositionByPoint(Point point, double scale)
		{
			var width = point.X * scale;
			if (width > content.ActualWidth * scale - ActualWidth / 2)
				width = content.ActualWidth * scale - ActualWidth / 2;
			if (width < ActualHeight/2)
				width = ActualWidth/2;

			var height = point.Y * scale;
			if (height > content.ActualHeight * scale - ActualHeight / 2)
				height = content.ActualHeight * scale - ActualHeight / 2;
			if (height < ActualHeight/2)
				height = ActualHeight/2;

			return new Point((width - ActualWidth/2)*-1, (height - ActualHeight/2)*-1);
		}

		private void ApplyScaleAndPosition(bool animate)
		{
			if (animate)
			{
				isAnimating = true;

				var story = new Storyboard();
				story.Children.Add(CreateAnimation(((CompositeTransform)content.RenderTransform).ScaleX, currentScale, content.RenderTransform, "ScaleX"));
				story.Children.Add(CreateAnimation(((CompositeTransform)content.RenderTransform).ScaleY, currentScale, content.RenderTransform, "ScaleY"));
				story.Children.Add(CreateAnimation(((CompositeTransform)content.RenderTransform).TranslateX, currentPosition.X, content.RenderTransform, "TranslateX"));
				story.Children.Add(CreateAnimation(((CompositeTransform)content.RenderTransform).TranslateY, currentPosition.Y, content.RenderTransform, "TranslateY"));

				story.Completed += delegate { isAnimating = false; };
				story.Begin();
			}
			else
			{
				ApplyScale();
				ApplyPosition();
			}
		}

		private Timeline CreateAnimation(double from, double to, DependencyObject target, string property)
		{
			var animation = new DoubleAnimation();
			animation.From = from;
			animation.To = to;
			animation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
			animation.EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut};

			Storyboard.SetTarget(animation, target);
#if WINRT
			Storyboard.SetTargetProperty(animation, property);
#else
			Storyboard.SetTargetProperty(animation, new PropertyPath(property));
#endif
			return animation; 
		}

		private void UpdateGeometry()
		{
			geometry.Rect = new Rect(0, 0, ActualWidth, ActualHeight);
		}
		
		private double currentScale = 1d;
		private Point currentPosition = new Point(0, 0);

		private void UpdatScale(double scaleFactor)
		{
			currentScale *= scaleFactor;
			ApplyScale();
		}

		private void UpdatePosition(Point delta)
		{
			var newPosition = new Point(currentPosition.X + delta.X, currentPosition.Y + delta.Y);

			if (newPosition.X > 0) newPosition.X = 0;
			if (newPosition.Y > 0) newPosition.Y = 0;

			if ((content.ActualWidth * currentScale) + newPosition.X < content.ActualWidth)
				newPosition.X = content.ActualWidth - (content.ActualWidth * currentScale);

			if ((content.ActualHeight * currentScale) + newPosition.Y < content.ActualHeight)
				newPosition.Y = content.ActualHeight - (content.ActualHeight * currentScale);

			currentPosition = newPosition;

			ApplyPosition();
		}

		public void ResetZoomAndPosition(bool animate)
		{
			currentScale = 1;
			currentPosition = new Point(0, 0);

			ApplyScaleAndPosition(animate);
		}

		private void ApplyScale()
		{
			((CompositeTransform)content.RenderTransform).ScaleX = currentScale;
			((CompositeTransform)content.RenderTransform).ScaleY = currentScale;
		}

		private void ApplyPosition()
		{
			((CompositeTransform)content.RenderTransform).TranslateX = currentPosition.X;
			((CompositeTransform)content.RenderTransform).TranslateY = currentPosition.Y;
		}

		private bool IsScaleValid(double scaleDelta)
		{
			return (currentScale * scaleDelta >= 1) && (currentScale * scaleDelta <= MaxZoomFactor);
		}
	}
}
