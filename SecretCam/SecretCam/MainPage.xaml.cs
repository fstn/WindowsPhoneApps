using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using FstnUserControl.Camera;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SecretCam.Resources;
using Windows.Phone.Media.Capture;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using FstnUserControl.Error;
using FstnUserControl;
using Microsoft.Expression.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.Windows.Threading;

namespace SecretCam
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CameraShooter primaryShooter;
        private ProgressIndicator indicator;
        private DispatcherTimer dt;
        private DispatcherTimer dt2;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Unloaded += Clean;
            this.BackKeyPress += MainPage_BackKeyPress;
            indicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true
            };
            SystemTray.SetProgressIndicator(this, indicator);
            CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt2 = new DispatcherTimer();
            dt2.Interval = TimeSpan.FromSeconds(1);


        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RootLayout.SelectedIndex > 0)
            {
                RootLayout.SelectedIndex = 0;
                e.Cancel = true;
            }
        }

        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (primaryShooter != null)
                primaryShooter.Orientation = this.Orientation;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            helpImage.Tap += helpImage_Tap;
            LoadShooter(CameraType.Primary);
        }

        void helpImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            helpImage.Tap -= helpImage_Tap;
            RootLayout.SelectedIndex = 1;
        }

        void LoadShooter(CameraType type)
        {
            primaryShooter = new CameraShooter(type);
            primaryCapture.Children.Add(primaryShooter);
            Canvas.SetLeft(primaryShooter, 5);
            Canvas.SetTop(primaryShooter, 5);
            primaryShooter.InitWidth = 600;
            primaryShooter.InitHeight = 0.75 * primaryShooter.InitWidth;
            primaryShooter.ListenTap();
            primaryShooter.Captured += primaryShooter_Captured;
            primaryShooter.Initialized += primaryShooter_Initialized;
            primaryShooter.Start();
            primaryShooter.Mute = true;
            maskImage.Tap += primaryCapture_Tap;
            this.Hold += AskToShare;
        }


        void primaryShooter_Initialized(object sender, EventArgs e)
        {
            primaryShooter.Camera.FlashMode = FlashMode.Off;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    indicator.IsVisible = false;
                });
        }
        void primaryShooter_Captured(object sender, object e)
        {
            primaryShooter.StopListenTap();
            OrientedCameraImage OrientedImage = (OrientedCameraImage)e;

            displayImage.Source = OrientedImage.Image;
            ScreenShot.Save(displayImage);
            displayImage.Visibility = Visibility.Visible;

            RecPt.Visibility = Visibility.Visible;
            dt2.Start();
            dt2.Tick += (o, ed) =>
            {
                RecPt.Visibility = Visibility.Collapsed;
                dt2.Stop();
            };
        }


        private void AskToSwitch(object sender, EventArgs e)
        {
            if (primaryShooter != null)
            {
                if (primaryShooter.Camera.CameraType == (CameraType.Primary))
                    LoadShooter(CameraType.FrontFacing);
                else
                    LoadShooter(CameraType.Primary);

            }
        }

        private void AskToShare(object sender, EventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(displayImage).GetPath();
            task.Show();
        }


        void primaryCapture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            maskImage.Visibility = Visibility.Collapsed;
            dt.Start();
            dt.Tick += (o, ed) =>
            {
                maskImage.Visibility = Visibility.Visible;
                dt.Stop();
            };
        }


        void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            doNewPics();
        }

        void doNewPics()
        {

            if (displayImage.Visibility == Visibility.Visible)
            {
                displayImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (primaryShooter != null && primaryShooter.Camera != null)
                {
                    primaryShooter.Focus();
                }
            }
        }

        void Clean(object sender, RoutedEventArgs e)
        {
            if (primaryShooter != null)
                primaryShooter.Stop();
        }

    }
}