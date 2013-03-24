﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MyToolkit.Controls
{
	public sealed class MenuListBox : Control
	{
		public MenuListBox()
		{
			DefaultStyleKey = typeof(MenuListBox);
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof (object), typeof (MenuListBox), new PropertyMetadata(default(object)));

		public object ItemsSource
		{
			get { return GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var items = (ExtendedItemsControl)GetTemplateChild("items");
			items.PrepareContainerForItem += ItemsOnPrepareContainerForItem;
		}

		private void ItemsOnPrepareContainerForItem(object sender, PrepareContainerForItemEventArgs args)
		{
			var element = ((FrameworkElement)args.Element);
			element.Tapped += OnTapped;
			element.Tag = args.Item;
		}

		public event EventHandler<NavigationListEventArgs> Navigate;

		private void OnTapped(object sender, TappedRoutedEventArgs args)
		{
			var copy = Navigate;
			if (copy != null)
				copy(this, new NavigationListEventArgs(((FrameworkElement)sender).Tag));
		}
	}
}
