using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using MyToolkit.Environment;
using MyToolkit.Paging;
using MyToolkit.Utilities;

namespace MyToolkit.UI.Popups
{
	public abstract class PopupControl : UserControl
	{
		#region Static

		public static Popup Show(PopupControl control)
		{
			return Show(control, false, false, null);
		}

		public static Popup Show(PopupControl control, bool hideApplicationBar, bool showFullScreen, Action<PopupControl> completed)
		{
			var oldState = new PageDeactivator();
			var page = PhonePage.CurrentPage;

			var heightSub = 0.0;
			if (hideApplicationBar && page.ApplicationBar.IsVisible)
			{
				page.ApplicationBar.IsVisible = false;
				heightSub = page.ApplicationBar.Mode == ApplicationBarMode.Minimized
								? page.ApplicationBar.MiniSize
								: page.ApplicationBar.DefaultSize;
			}
			else
				hideApplicationBar = false;

			var content = page.Content;
			content.IsHitTestVisible = false;

			if (showFullScreen && content.Visibility == Visibility.Visible)
				content.Visibility = Visibility.Collapsed;
			else
				showFullScreen = false; 

			var popup = new Popup
			{
				Child = control,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};

			control.Popup = popup;

			if (showFullScreen)
				popup.Height = page.ActualHeight + heightSub;
			popup.Width = page.ActualWidth;

			if (showFullScreen)
				control.Height = page.ActualHeight + heightSub;
			control.Width = page.ActualWidth;

			var color = ColorUtility.RemoveAlpha(
				PhoneApplication.IsDarkTheme ? ColorUtility.FromHex("#22FFFFFF") :
				ColorUtility.FromHex("#DDFFFFFF"), Colors.Black);

			var oldColor = SystemTray.BackgroundColor;

			control.SetBackgroundColor(color);
			oldState.DoIt(false);

			var del = new EventHandler<CancelEventArgs>(delegate(object sender, CancelEventArgs args)
			{
				args.Cancel = true;
				control.GoBack();
			});
			page.BackKeyPress += del;

			SystemTray.BackgroundColor = color;

			popup.IsOpen = true;
			control.Closed += delegate
			{
				if (showFullScreen)
					content.Visibility = Visibility.Visible;

				if (hideApplicationBar)
					page.ApplicationBar.IsVisible = true;

				content.IsHitTestVisible = true;

				popup.IsOpen = false;
				oldState.Revert();

				page.BackKeyPress -= del;
				SystemTray.BackgroundColor = oldColor;

				if (completed != null)
					completed(control);
			};

			return popup;
		}

		#endregion

		public abstract void GoBack();

		public event Action<PopupControl> Closed;
		public Popup Popup { get; internal set; }
		
		public virtual void SetBackgroundColor(Color color)
		{
			if (Content is Control)
				((Control)Content).Background = new SolidColorBrush(color);
			else if (Content is Panel)
				((Panel)Content).Background = new SolidColorBrush(color);
			else
				throw new NotImplementedException();
		}

		public void Close()
		{
			var copy = Closed;
			if (copy != null)
				Closed(this);
		}

		public void Show()
		{
			Show(this);
		}
	}
}