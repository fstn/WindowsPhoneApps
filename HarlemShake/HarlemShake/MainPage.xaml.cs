using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using HarlemShake.Resources;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System.Xml.Linq;
using FstnUserControl.Video;
using ShakeGestures;
using System.Windows.Navigation;

namespace HarlemShake
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CameraShooter primaryShooter;
        private ProgressIndicator indicator;
        private ShakeGesturesHelper shake;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitAppBar();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                indicator = new ProgressIndicator
                {
                    IsVisible = true,
                    IsIndeterminate = true
                };
                SystemTray.SetProgressIndicator(this, indicator);
                CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;
                LoadShooter(CameraType.FrontFacing);

                var wc = new WebClient();
                wc.DownloadStringCompleted += DownloadStringCompleted;
                var searchUri = string.Format(
                  "http://gdata.youtube.com/feeds/api/videos?q={0}&format=6",
                  HttpUtility.UrlEncode("harlem shake"));
                wc.DownloadStringAsync(new Uri(searchUri));
                shake = ShakeGesturesHelper.Instance;
                shake.ShakeGesture += shake_ShakeGesture;
                RootLayout.SelectionChanged += RootLayout_SelectionChanged;
            }
            catch (Exception ex)
            {
            }
        }

        void RootLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RootLayout.SelectedIndex == 2)
            {
                shake.Active = true;
            }
            else
            {
                shake.Active = false;
            }
        }

        void shake_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            VibrateController vc = VibrateController.Default;
            vc.Start(TimeSpan.FromMilliseconds(1000));

        }


        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RootLayout.SelectedIndex > 0)
            {
                RootLayout.SelectedIndex = 0;
                e.Cancel = true;
            }

            if ((primaryShooter != null && primaryShooter.CameraIsInitialized == false))
            {
                e.Cancel = true;
            }
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
            primaryShooter.Orientation = PageOrientation.PortraitUp;
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
            displayImage.RenderTransformOrigin = new Point(0.5, 0.5);
            ScreenShot.Save(exportCanvas);
            displayImage.Visibility = Visibility.Visible;
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


        private void AskToShare(object sender, EventArgs e)
        {
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(exportCanvas).GetPath();
            task.Show();
        }

        void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            try
            {
                var atomns = XNamespace.Get("http://www.w3.org/2005/Atom");
                var medians = XNamespace.Get("http://search.yahoo.com/mrss/");
                var xml = XElement.Parse(e.Result);
                if (xml != null)
                {
                    var videos = (
                      from entry in xml.Descendants(atomns.GetName("entry"))
                      select new YoutubeVideo
                      {
                          VideoId = entry.Element(atomns.GetName("id")).Value,
                          VideoImageUrl = (
                            from thumbnail in entry.Descendants(medians.GetName("thumbnail"))
                            where thumbnail.Attribute("height").Value == "360"
                            select thumbnail.Attribute("url").Value).FirstOrDefault(),
                          VideoUrl = (
                          from player in entry.Descendants(medians.GetName("player"))
                          select player.Attribute("url").Value).FirstOrDefault(),
                          Title = entry.Element(atomns.GetName("title")).Value
                      }).ToArray();
                    ResultsList.ItemsSource = videos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Networking problems, please check your connection");
            }
        }
        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Save, AskToShare);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.png", Msg.Back, AskToBack);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.flash.png", Msg.Back, AskToFlash);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (RootLayout.SelectedIndex != 0 || (primaryShooter != null && primaryShooter.Camera == null))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?t=" + RandomService.Instance.getRand(), UriKind.Relative));
            }
        }
    }
    public class YouTubeVideo
    {
    }
}