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
using FstnUserControl.Error;
using FstnUserControl;
using Microsoft.Expression.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;

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
            CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;             
        }


        void RootLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RootLayout.SelectedIndex == 2)
            {
                switch (primaryDisplayer.OrientedImage.Orientation)
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
        private WriteableBitmap CreateExport()
        {
            if (primaryDisplayer != null && primaryDisplayer.OrientedImage != null
                && secondDisplayer != null && secondDisplayer.OrientedImage != null)
            {
                WriteableBitmap backBitmap = primaryDisplayer.OrientedImage.Image;
                WriteableBitmap frontBitmap = secondDisplayer.OrientedImage.Image;

                double newSize = 0.0;
                if (frontBitmap.PixelWidth > frontBitmap.PixelHeight)
                {

                    if (backBitmap.PixelWidth > backBitmap.PixelHeight)
                    {
                        newSize = secondDisplayer.InitWidth / primaryDisplayer.InitWidth * backBitmap.PixelWidth;
                        frontBitmap = WriteableBitmapExtensions.Resize(frontBitmap, (int)newSize, (int)(0.75 * newSize), WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                    }
                    else
                    {
                        newSize = secondDisplayer.InitWidth / primaryDisplayer.InitWidth * backBitmap.PixelHeight;
                        frontBitmap = WriteableBitmapExtensions.Resize(frontBitmap, (int)newSize, (int)(0.75 * newSize), WriteableBitmapExtensions.Interpolation.NearestNeighbor);

                    }
                }
                else
                {
                    if (backBitmap.PixelWidth > backBitmap.PixelHeight)
                    {
                        newSize = secondDisplayer.InitWidth / primaryDisplayer.InitWidth * backBitmap.PixelWidth;
                        frontBitmap = WriteableBitmapExtensions.Resize(frontBitmap, (int)(0.75 * newSize), (int)newSize, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                    }
                    else
                    {
                        newSize = secondDisplayer.InitWidth / primaryDisplayer.InitWidth * backBitmap.PixelHeight;
                        frontBitmap = WriteableBitmapExtensions.Resize(frontBitmap, (int)(0.75 * newSize), (int)newSize, WriteableBitmapExtensions.Interpolation.NearestNeighbor);

                    }
                }
                Point destPoint = secondDisplayer.PercentPosition;
                destPoint.X = destPoint.X * backBitmap.PixelWidth;
                destPoint.Y = destPoint.Y * backBitmap.PixelHeight;
                Rect frontRect = new Rect() { X = 0, Y = 0, Width = frontBitmap.PixelWidth, Height = frontBitmap.PixelHeight };
                WriteableBitmapExtensions.Blit(backBitmap,
                                               destPoint,
                                               frontBitmap,
                                               frontRect,
                                               Colors.White,
                                               WriteableBitmapExtensions.BlendMode.None);
                return backBitmap;
            }
            else
            {
                return null;
            }
        }
        private void AskToShare(object sender, EventArgs e)
        {
            WriteableBitmap backBitmap = CreateExport();
            if (backBitmap != null)
            {
                var width = (int)backBitmap.PixelWidth;
                var height = (int)backBitmap.PixelHeight;
                using (var ms = new MemoryStream(width * height * 4))
                {
                    backBitmap.SaveJpeg(ms, width, height, 0, 100);
                    ms.Seek(0, SeekOrigin.Begin);
                    var lib = new MediaLibrary();
                    var name = String.Format("{0:yyyy-MM-dd_hh-mm-ss}.jpg", DateTime.Now);
                    var task = new ShareMediaTask();
                    try
                    {
                        var picture = lib.SavePicture(String.Format(name), ms);
                        task.FilePath = picture.GetPath();
                    }
                    catch (NullReferenceException ex)
                    {
                        Debugger.Log(4, "", ex.Message);
                    }
                    task.Show();
                }
            }
        }

        private void AskToSave(object sender, EventArgs e)
        {
            WriteableBitmap backBitmap = CreateExport();
            if (backBitmap != null)
            {
                var width = (int)backBitmap.PixelWidth;
                var height = (int)backBitmap.PixelHeight;
                using (var ms = new MemoryStream(width * height * 4))
                {
                    backBitmap.SaveJpeg(ms, width, height, 0, 100);
                    ms.Seek(0, SeekOrigin.Begin);
                    var lib = new MediaLibrary();
                    var name = String.Format("{0:yyyy-MM-dd_hh-mm-ss}.jpg", DateTime.Now);
                    try
                    {
                        var picture = lib.SavePicture(String.Format(name), ms);
                    }
                    catch (NullReferenceException ex)
                    {
                        Debugger.Log(4, "", ex.Message);
                    }
                }
                MessageBox.Show(Msg.ImageSaved);
            }
        }

        private void InitDisplayer()
        {
            primaryDisplayer = new CameraImageDisplayer(new Rectangle())
            {
                InitWidth = 650,
                InitHeight = 400
            };
            finalPicture.Children.Add(primaryDisplayer);
            Rectangle p = new Rectangle();
            secondDisplayer = new CameraImageDisplayer(p)
            {
                InitWidth = 160,
                InitHeight = 120,
                Type = CameraType.FrontFacing,
                TopLeft = new Point(0, 0),
                BottomRight = new Point(primaryDisplayer.InitWidth, primaryDisplayer.InitWidth * 0.75)
            };
            finalPicture.Children.Add(secondDisplayer);
        }

        void MovePhotographerTo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point pointToMove = new Point(e.GetPosition(finalPicture).X,
                                          e.GetPosition(finalPicture).Y);
            secondDisplayer.MoveTo(pointToMove);
        }

        void primaryShooter_Captured(object sender, object e)
        {

            InitDisplayer();
            primaryShooter.StopListenTap();
            Canvas.SetLeft(primaryDisplayer, 0);
            Canvas.SetTop(primaryDisplayer, 0);
            primaryDisplayer.DisplayOrientation = this.Orientation;
            Canvas.SetLeft(secondDisplayer, 0);
            Canvas.SetTop(secondDisplayer, 0);
            secondDisplayer.DisplayOrientation = this.Orientation;
            try
            {
                primaryDisplayer.Load((OrientedCameraImage)e);
            }
            catch (InvalidCastException ex)
            {
                ErrorService.Instance.AddError(this, "", ErrorType.CANT_CAST_ORIENTED, ex);
            }

            secondShooter.StartAndShoot();
            RootLayout.IsLocked = false;
            RootLayout.SelectedIndex = 1;
            RootLayout.IsLocked = true;
        }

        private void secondShooter_Captured(object sender, object e)
        {
            try
            {
                secondDisplayer.Load((OrientedCameraImage)e);
            }
            catch (InvalidCastException ex)
            {
                ErrorService.Instance.AddError(this, "", ErrorType.CANT_CAST_ORIENTED, ex);
            }
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
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.png", Msg.Back, AskToBack);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.camera.flash.png", Msg.Back, AskToFlash);
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
            if (RootLayout.SelectedIndex != 0)
            {
                try
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml?t=" + RandomService.Instance.getRand(), UriKind.Relative));
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            else
            {
                if (primaryShooter != null && primaryShooter.Camera != null)
                {
                    primaryShooter.Focus();
                }
            }
        }

        void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            if (primaryShooter != null && primaryShooter.Camera != null)
            {
                primaryShooter.Focus();
            }
        }

        void Clean(object sender, RoutedEventArgs e)
        {
            if (secondShooter != null)
                secondShooter.Stop();
            if (primaryShooter != null)
                primaryShooter.Stop();
            primaryDisplayer = null;
            secondDisplayer = null;
        }

    }
}