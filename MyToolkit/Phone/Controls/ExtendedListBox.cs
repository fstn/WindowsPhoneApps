﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using MyToolkit.UI;

#if WP7 || WP8
using Microsoft.Phone.Controls;
#endif

// developed by Rico Suter (http://rsuter.com), http://mytoolkit.codeplex.com

namespace MyToolkit.Controls
{
	public class ScrollingStateChangedEventArgs : EventArgs
	{
		public bool OldValue { get; private set; }
		public bool NewValue { get; private set; }
		public ScrollingStateChangedEventArgs(bool oldValue, bool newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}

	public class ScrolledToEndEventArgs :EventArgs
	{
		public ScrollViewer ScrollViewer { get; set; }
		public ScrolledToEndEventArgs(ScrollViewer viewer)
		{
			ScrollViewer = viewer;
		}
	}

	public class ExtendedListBox : ListBox
	{
		public ExtendedListBox()
		{
			//DefaultStyleKey = typeof(ExtendedListBox);
			LayoutUpdated += RegisterScrollEvent;
			ItemContainerStyle = (Style)XamlReader.Load(
				@"<Style TargetType=""ListBoxItem"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
					<Setter Property=""Template"">
						<Setter.Value>
							<ControlTemplate>
								<ContentPresenter HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch""/>
							</ControlTemplate>
						</Setter.Value>
					</Setter>
				</Style>");
		}

		private ScrollViewer scrollViewer;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");

			UpdateInnerMargin();
			RegisterScrollOffset();
		}

		public ListBoxItem GetListBoxItemFromItem(object item)
		{
			return (ListBoxItem)ItemContainerGenerator.ContainerFromItem(item);
		}

		public ScrollViewer ScrollViewer
		{
			get { return scrollViewer; }
		}

		public bool ScrollToVerticalOffset(double offset)
		{
			if (scrollViewer != null)
			{
				scrollViewer.InvalidateScrollInfo();
				scrollViewer.ScrollToVerticalOffset(offset);
				return true; 
			}
			return false;
		}

		public bool StopScrolling()
		{
			if (scrollViewer != null)
			{
				scrollViewer.InvalidateScrollInfo();
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
				return true;
			}
			return false;
		}

		#region scroll jumping fix

#if WP8 || WP7

		public static readonly DependencyProperty UseScrollFixProperty =
			DependencyProperty.Register("UseScrollFix", typeof(bool), typeof(ExtendedListBox), new PropertyMetadata(true));

		public bool UseScrollFix
		{
			get { return (bool) GetValue(UseScrollFixProperty); }
			set { SetValue(UseScrollFixProperty, value); }
		}

		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			if (UseScrollFix)
			{
				var page = (PhoneApplicationPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
				page.Focus();
			}

			base.OnManipulationCompleted(e);
		}

		//protected override void OnGotFocus(RoutedEventArgs e)
		//{
		//    if (UseScrollFix && !(e.OriginalSource is Button)) // disable SetFocus if clicked on Button, otherwise the Button will not work properly
		//    {
		//        var page = (PhoneApplicationPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
		//        page.Focus();
		//    }

		//    base.OnGotFocus(e);
		//}

#endif

		#endregion

		#region scroll to end

		public bool TriggerScrolledToEndEvents { get; set; }

		private event EventHandler<ScrolledToEndEventArgs> scrolledToEnd;
		public event EventHandler<ScrolledToEndEventArgs> ScrolledToEnd
		{
			add 
			{ 
				scrolledToEnd += value;
				RegisterScrollOffset();
			}
			remove { scrolledToEnd -= value; }
		}

		private double lastViewportHeight = 0; 
		private static void OnListVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (ExtendedListBox) d;
			if (!ctrl.TriggerScrolledToEndEvents || ctrl.scrolledToEnd == null)
				return;

			var viewer = ctrl.scrollViewer;
			if (viewer != null)
			{
				var triggered = ctrl.lastViewportHeight == viewer.ViewportHeight;
				if (!triggered && viewer.VerticalOffset >= viewer.ScrollableHeight - viewer.ViewportHeight - viewer.ViewportHeight / 2)
				{
					ctrl.lastViewportHeight = viewer.ViewportHeight;
					var handler = ctrl.scrolledToEnd;
					if (handler != null)
						handler(ctrl, new ScrolledToEndEventArgs(viewer));
				}
			}
		}

		private static readonly DependencyProperty InternalOffsetProperty = DependencyProperty.Register(
			"InternalOffset", typeof(double), typeof(ExtendedListBox),
			new PropertyMetadata(default(double), OnListVerticalOffsetChanged));

		private bool bindingCreated = false;
		private void RegisterScrollOffset()
		{
			if (scrollViewer == null || bindingCreated || scrolledToEnd == null)
				return;

			TriggerScrolledToEndEvents = true;

			var binding = new Binding();
			binding.Source = scrollViewer;
			binding.Path = new PropertyPath("VerticalOffset");
			binding.Mode = BindingMode.OneWay;
			SetBinding(InternalOffsetProperty, binding);
		
			bindingCreated = true;
		}

		#endregion

		#region inner margin

		public Thickness InnerMargin
		{
			get { return (Thickness)GetValue(InnerMarginProperty); }
			set { SetValue(InnerMarginProperty, value); }
		}

		public static readonly DependencyProperty InnerMarginProperty =
			DependencyProperty.Register("InnerMargin", typeof(Thickness),
			typeof(ExtendedListBox), new PropertyMetadata(new Thickness(), InnerMarginChanged));

		private static void InnerMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var box = (ExtendedListBox)d;
			box.UpdateInnerMargin();
		}

		private void UpdateInnerMargin()
		{
			UpdateLastItemMargin();
			if (scrollViewer != null)
			{
				var itemsPresenter = (ItemsPresenter)scrollViewer.Content;
				if (itemsPresenter != null)
					itemsPresenter.Margin = InnerMargin;
			}
		}

		private void UpdateLastItemMargin()
		{
			if (lastElement != null)
			{
				lastElement.Margin = new Thickness(lastElementMargin.Left, lastElementMargin.Top, lastElementMargin.Right,
					lastElementMargin.Bottom + InnerMargin.Top + InnerMargin.Bottom);
			}
		}

		private FrameworkElement lastElement = null;
		private Thickness lastElementMargin;
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			OnPrepareContainerForItem(new PrepareContainerForItemEventArgs(element, item));

			if ((InnerMargin.Top > 0.0 || InnerMargin.Bottom > 0.0))
			{
				if (Items.IndexOf(item) == Items.Count - 1) // is last element of list
				{
					if (lastElement != element) // margin not already set
					{
						if (lastElement != null)
							lastElement.Margin = lastElementMargin;
						lastElement = (FrameworkElement)element;
						lastElementMargin = lastElement.Margin;
						UpdateLastItemMargin();
					}
				}
				else if (lastElement == element) // if last element is recycled it appears inside the list => reset margin
				{
					lastElement.Margin = lastElementMargin;
					lastElement = null; 
				}
			}
		}

		#endregion

		#region prepare container for item event

		public event EventHandler<PrepareContainerForItemEventArgs> PrepareContainerForItem;
		protected void OnPrepareContainerForItem(PrepareContainerForItemEventArgs args)
		{
			var copy = PrepareContainerForItem;
			if (copy != null)
				copy(this, args);
		}

		#endregion

		#region scrolling

		public event EventHandler<ScrollingStateChangedEventArgs> ScrollingStateChanged;

		public static readonly DependencyProperty IsScrollingProperty = 
			DependencyProperty.Register("IsScrolling", typeof(bool),
			typeof(ExtendedListBox), new PropertyMetadata(false, IsScrollingPropertyChanged));

		private bool allowIsScrollingChanges;
		public bool IsScrolling
		{
			get { return (bool) GetValue(IsScrollingProperty); }
			internal set
			{
				// "Unlock" the ability to set the property
				allowIsScrollingChanges = true;
				SetValue(IsScrollingProperty, value);
				allowIsScrollingChanges = false;
			}
		}

		protected virtual void OnScrollingStateChanged(ScrollingStateChangedEventArgs e) { }

		internal static void IsScrollingPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			var listbox = (ExtendedListBox) source;
			if (listbox.allowIsScrollingChanges != true)
				throw new InvalidOperationException("IsScrolling property is read-only");

			var args = new ScrollingStateChangedEventArgs((bool)e.OldValue, (bool)e.NewValue);
			listbox.OnScrollingStateChanged(args);
			if (listbox.ScrollingStateChanged != null)
				listbox.ScrollingStateChanged(listbox, args);
		}

		void ScrollingStateChanging(object sender, VisualStateChangedEventArgs e)
		{
			IsScrolling = (e.NewState.Name == "Scrolling");
		}

		private bool eventRegistred = false; 
		private void RegisterScrollEvent(object s, EventArgs eventArgs)
		{
			if (eventRegistred)
				return; 

			if (scrollViewer != null)
			{
				var child = scrollViewer.GetVisualChild(0);
				var group = child.GetVisualStateGroup("ScrollStates");
				if (group != null)
				{
					group.CurrentStateChanging -= ScrollingStateChanging;
					group.CurrentStateChanging += ScrollingStateChanging;
					eventRegistred = true; 
				}
			}
		}

		#endregion
	}
}
