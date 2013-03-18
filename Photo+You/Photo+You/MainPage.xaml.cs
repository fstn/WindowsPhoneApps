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
using Photo_You.Resources;
using Windows.Phone.Media.Capture;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace Photo_You
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CameraShooter secondShooter;
        private CameraShooter primaryShooter;
        private CameraImageDisplayer primaryDisplayer;
        private CameraImageDisplayer secondDisplayer;
        private ProgressIndicator indicator;
        // Constructor
        public MainPage()
        {
            InitAppBar();
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Unloaded += Clean;
            RootLayout.SelectionChanged += RootLayout_SelectionChanged;
            RootLayout.IsLocked = true;
            this.BackKeyPress += MainPage_BackKeyPress;
            indicator = new ProgressIndicator
            {
                IsVisible = true,
                IsIndeterminate = true
            };
            SystemTray.SetProgressIndicator(this, indicator);
        }

        void RootLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RootLayout.SelectedIndex == 2)
            {
                switch (primaryDisplayer.PictureOrientation)
                {
                    case PageOrientation.Landscape:
                        this.SupportedOrientations = SupportedPageOrientation.Landscape;
                        break;
                    case PageOrientation.LandscapeLeft:
                        this.SupportedOrientations = SupportedPageOrientation.Landscape;
                        break;
                    case PageOrientation.LandscapeRight:
                        this.SupportedOrientations = SupportedPageOrientation.Landscape;
                        break;
                    case PageOrientation.Portrait:
                        this.SupportedOrientations = SupportedPageOrientation.Portrait;
                        break;
                    case PageOrientation.PortraitUp:
                        this.SupportedOrientations = SupportedPageOrientation.Portrait;
                        break;
                    case PageOrientation.PortraitDown:
                        this.SupportedOrientations = SupportedPageOrientation.Portrait;
                        break;

                }
            }
            else
            {
                this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
            }
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
            if (secondShooter != null)
                secondShooter.Orientation = this.Orientation;
        /*    if (primaryDisplayer != null)
                primaryDisplayer.Orientation = this.Orientation;
            if (secondDisplayer != null)
                secondDisplayer.Orientation = this.Orientation;*/
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            primaryShooter = new CameraShooter(CameraType.Primary);
            primaryCapture.Children.Add(primaryShooter);
            primaryShooter.InitWidth = 650;
            primaryShooter.InitHeight = 0.75 * primaryShooter.InitWidth;
            primaryShooter.ListenTap();
            primaryShooter.Captured += primaryShooter_Captured;
            primaryShooter.Initialized += primaryShooter_Initialized;
            primaryShooter.Start();

            secondShooter = new CameraShooter(CameraType.FrontFacing);
            secondCapture.Children.Add(secondShooter);
            secondShooter.InitWidth = 650;
            secondShooter.InitHeight = 0.75 * secondShooter.InitWidth;
            secondShooter.Captured += secondShooter_Captured;

            finalPicture.Tap += MovePhotographerTo;
            InitDisplayer();
            MainPage_OrientationChanged(null, null);
            this.OrientationChanged += MainPage_OrientationChanged;
        }

        void primaryShooter_Initialized(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    indicator.IsVisible = false;
                });
        }

        private void AskToShare(object sender, EventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(finalPicture).GetPath();
            task.Show();
        }

        private void AskToSave(object sender, EventArgs e)
        {
            ScreenShot.Take(finalPicture).GetPath();
        }

        private void InitDisplayer()
        {
            primaryDisplayer = new CameraImageDisplayer(new Rectangle())
            {
                InitWidth = 600,
                InitHeight = 450
            };
            finalPicture.Children.Add(primaryDisplayer);
            secondDisplayer = new CameraImageDisplayer(new Rectangle())
            {
                InitWidth = 160,
                InitHeight = 120,
                Type = CameraType.FrontFacing,
                TopLeft = new Point(0, 0),
                BottomRight = new Point(primaryDisplayer.Width, primaryDisplayer.Height)
            };
            finalPicture.Children.Add(secondDisplayer);

        }

        void MovePhotographerTo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point pointToMove = new Point(e.GetPosition(secondDisplayer).X,
                                          e.GetPosition(secondDisplayer).Y);
            secondDisplayer.MoveTo(pointToMove);
        }

        void primaryShooter_Captured(object sender, System.Windows.Media.Imaging.BitmapImage e)
        {
            primaryShooter.StopListenTap();
            primaryDisplayer.Load(e, this.Orientation);
            primaryDisplayer.PictureOrientation = this.Orientation;
            secondDisplayer.PictureOrientation = this.Orientation;
            secondShooter.StartAndShoot();
            RootLayout.IsLocked = false;
            RootLayout.SelectedIndex = 1;
            RootLayout.IsLocked = true;
        }

        private void secondShooter_Captured(object sender, System.Windows.Media.Imaging.BitmapImage e)
        {
            secondDisplayer.Load(e, this.Orientation);

            RootLayout.IsLocked = false;
            RootLayout.SelectedIndex = 2;
            RootLayout.IsLocked = true;
            secondShooter.Stop();
            primaryShooter.Start();
            primaryShooter.ListenTap();
        }

        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.save.png", Msg.Save, AskToSave);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.back.png", Msg.Back, AskToBack);
        }

        private void AskToBack(object sender, EventArgs e)
        {
            if (RootLayout.SelectedIndex != 0)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?t=q", UriKind.Relative));
            }
        }


        void Clean(object sender, RoutedEventArgs e)
        {
            if (secondShooter != null)
                secondShooter.Stop();
            if (primaryShooter != null)
                primaryShooter.Stop();
        }

    }
}