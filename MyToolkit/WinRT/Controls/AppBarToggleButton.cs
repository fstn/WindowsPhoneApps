﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MyToolkit.Controls
{
	public class AppBarToggleButton : ToggleButton
	{
		public AppBarToggleButton()
		{
			DefaultStyleKey = typeof(AppBarToggleButton);
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(String), typeof(AppBarToggleButton), new PropertyMetadata(default(String)));

		public String Header
		{
			get { return (String) GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
	}
}