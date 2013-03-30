using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using FstnCommon.Util;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using FstnUserControl.Camera;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using HeartCam.Resources;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace HeartCam
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CameraShooter primaryShooter;
        private ProgressIndicator indicator;
        // Constructor
        public MainPage()
        {
            InitAppBar();
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Unloaded += Clean;
            RootLayout.IsLocked = true;
            this.BackKeyPress += MainPage_BackKeyPress;
            indicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true
            };
            SystemTray.SetProgressIndicator(this, indicator);
            CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RootLayout.SelectedIndex > 0)
            {
                RootLayout.IsLocked = false;
                RootLayout.SelectedIndex = 0;
                RootLayout.IsLocked = true;
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
            LoadShooter(CameraType.Primary);
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
        }

        void primaryShooter_Initialized(object sender, EventArgs e)
        {
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
            ScreenShot.Save(exportCanvas);
            displayImage.Visibility = Visibility.Visible;
        }

        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.flash.png", Msg.Flash, AskToFlash);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.png", Msg.Back, AskToBack);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.arrow.left.right.png", Msg.Switch, AskToSwitch);
        }

        private void AskToSwitch(object sender, EventArgs e)
        {
            try
            {
                if (primaryShooter != null)
                {
                    CameraType type = primaryShooter.Camera.CameraType;

                    primaryCapture.Children.Remove(primaryShooter);
                    primaryShooter.Captured -= primaryShooter_Captured;
                    primaryShooter.Initialized -= primaryShooter_Initialized;
                    primaryShooter.Stop();
                    if (type == (CameraType.Primary))
                        LoadShooter(CameraType.FrontFacing);
                    else
                        LoadShooter(CameraType.Primary);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry can change of cam");
            }
        }

        private void AskToFlash(object sender, EventArgs e)
        {
            try
            {
                if (primaryShooter.Camera != null)
                {
                    if (primaryShooter.Camera.FlashMode == FlashMode.RedEyeReduction
                        && primaryShooter.Camera.IsFlashModeSupported(FlashMode.RedEyeReduction))
                    {
                        primaryShooter.Camera.FlashMode = FlashMode.Auto;
                        MessageBox.Show(Msg.Auto);
                    }
                    else if (primaryShooter.Camera.FlashMode == FlashMode.Auto
                        && primaryShooter.Camera.IsFlashModeSupported(FlashMode.Auto))
                    {
                        primaryShooter.Camera.FlashMode = FlashMode.On;
                        MessageBox.Show(Msg.On);
                    }
                    else if (primaryShooter.Camera.FlashMode == FlashMode.On
                        && primaryShooter.Camera.IsFlashModeSupported(FlashMode.On))
                    {
                        primaryShooter.Camera.FlashMode = FlashMode.Off;
                        MessageBox.Show(Msg.Off);
                    }
                    else if (primaryShooter.Camera.FlashMode == FlashMode.Off
                        && primaryShooter.Camera.IsFlashModeSupported(FlashMode.Off))
                    {
                        primaryShooter.Camera.FlashMode = FlashMode.RedEyeReduction;
                        MessageBox.Show(Msg.RedEye);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Debugger.Log(0, "", "can't change flash mode");
            }
        }

        private void AskToShare(object sender, EventArgs e)
        {
            try
            {
                ShareMediaTask task = new ShareMediaTask();
                task.FilePath = ScreenShot.Take(exportCanvas).GetPath();
                task.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry but we can't share at the moment");
            }
        }

        private void AskToBack(object sender, EventArgs e)
        {
            doNewPics();
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