﻿using System;
using System.Collections;

#if !WINRT
using System.Windows;
using System.Windows.Data;
#else
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
#endif


namespace MyToolkit.Converters
{
	public class EqualityConverter : IValueConverter
	{
#if !WINRT
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
        public object Convert(object value, Type typeName, object parameter, string language)
#endif		
		{
			if (value != null && value == parameter)
				return Visibility.Visible;
			return Visibility.Collapsed;
		}

#if !WINRT
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#else
        public object ConvertBack(object value, Type typeName, object parameter, string language)
#endif
		{
			throw new NotSupportedException();
		}
	}
}
