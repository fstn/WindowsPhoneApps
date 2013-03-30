using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FstnCommon;
using FstnCommon.Util;
using Microsoft.Devices;
using System.IO;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using Windows.Phone.Devices.Notification;

namespace FstnUserControl.Camera
{
    public class CameraShooter : Canvas
    {
        public event InitializedEventHandler Initialized;
        public event CaptureEventHandler Captured;
        public PhotoCamera Camera { get; set; }
        public CameraType Type { get; set; }

        private DispatcherTimer dt;
        private VideoBrush brush;
        private CompositeTransform videoBrushTransform;

        private bool dtWasRunning = false;
        private bool camWasRunning = false;
        private bool busy = false;
        private bool mute = false;
        private bool cameraIsInitialized = false;
        public bool CameraIsInitialized
        {
            get
            {
                return cameraIsInitialized;
            }
        }

        private TextBlock decountText;
        private int decountNumber = 3;

        private SoundController sc;
        private Size resolution;
        private PageOrientation orientation = PageOrientation.LandscapeLeft;
        private double angle = 0;
        private Binding HeightBinding;
        private Binding WidthBinding;

        public bool Mute
        {
            get { return mute; }
            set { mute = value; }
        }

        private double initWidth;
        public double InitWidth
        {
            get
            {
                return initWidth;
            }
            set
            {
                initWidth = value;
                Width = value;
            }
        }

        private double initHeight;
        public double InitHeight
        {
            get
            {
                return initHeight;
            }
            set
            {
                initHeight = value;
                Height = value;
            }
        }
        public PageOrientation Orientation
        {
            set
            {
                orientation = value;
                UpdateOrientation();
            }
        }
        public CameraShooter(CameraType type)
        {
            try
            {
                if (PhotoCamera.IsCameraTypeSupported(type) == true)
                {
                    AppManager.Instance.Activated += Instance_Activated;
                    AppManager.Instance.Deactivated += Instance_Deactivated;
                    AppManager.Instance.Closed += Instance_Closed;
                    this.Type = type;
                    InitDT();
                    decountText = new TextBlock();
                    decountText.Text = "";
                    decountText.FontSize = 300;
                    decountText.TextAlignment = TextAlignment.Center;
                    decountText.VerticalAlignment = VerticalAlignment.Center;
                    this.Children.Add(decountText);
                    sc = new SoundController();

                    brush = new VideoBrush();
                    videoBrushTransform = new CompositeTransform();
                    videoBrushTransform.CenterX = .5;
                    videoBrushTransform.CenterY = .5;

                    if (type == CameraType.FrontFacing)
                    {
                        videoBrushTransform.ScaleX = -1;
                    }

                    brush.RelativeTransform = videoBrushTransform;

                    HeightBinding = new Binding();
                    HeightBinding.Source = this;
                    HeightBinding.Path = new PropertyPath("Height");

                    WidthBinding = new Binding();
                    WidthBinding.Source = this;
                    WidthBinding.Path = new PropertyPath("Width");

                    this.Background = brush;
                }
            }
            catch (Exception ex) { }
        }

        void Instance_Activated(object sender, EventArgs e)
        {
            /*
            try
            {
                if (camWasRunning)
                    Start();
                if (dtWasRunning)
                    dt.Start();
            }
            catch (Exception ex)
            {
            }
             * */
        }

        void Instance_Closed(object sender, EventArgs e)
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
            }
        }

        void Instance_Deactivated(object sender, EventArgs e)
        {
            try
            {
                if (Camera != null)
                    camWasRunning = true;
                else
                    camWasRunning = false;

                if (dt != null)
                    dt.Stop();
                if (dt.IsEnabled)
                {
                    dtWasRunning = true;
                }
                Stop();
            }
            catch (Exception ex)
            {
            }
        }

        private void InitDT()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(0.8);
            dt.Tick += (s, e) =>
            {
                if (!mute)
                    sc.PlaySound("Assets/Audio/bip.wav");
                if (decountNumber > 0)
                {
                    if (decountNumber > 2)
                        decountText.Foreground = new SolidColorBrush(Colors.Green);
                    else if (decountNumber > 1)
                        decountText.Foreground = new SolidColorBrush(Colors.Orange);
                    else
                        decountText.Foreground = new SolidColorBrush(Colors.Red);

                    decountText.Text = decountNumber.ToString();
                }
                else
                {
                    Focus();
                    dt.Stop();
                    decountText.Text = "";
                    decountNumber = 3;
                }
                decountNumber--;
            };
        }

        public void StartAndShoot()
        {
            decountText.SetBinding(TextBlock.HeightProperty, HeightBinding);
            decountText.SetBinding(TextBlock.WidthProperty, WidthBinding);
            decountText.Width = Width;
            decountText.Height = Height;
            decountText.Text = decountNumber.ToString();
            dt.Start();
            Start();
        }

        public void Start()
        {
            try
            {
                if (Camera != null)
                    Stop();
                Camera = new PhotoCamera(Type);
                Camera.Initialized += cam_Initialized;
                Camera.AutoFocusCompleted += AutoFocusCompleted;
                Camera.CaptureCompleted += CaptureCompleted;
                Camera.CaptureImageAvailable += CaptureImageAvailable;
                brush.SetSource(Camera);
            }
            catch (Exception) { }
        }

        public void Stop()
        {
            try
            {
                if (dt != null)
                    dt.Stop();
                if (Camera != null && cameraIsInitialized)
                {
                    try
                    {
                        Camera.Initialized -= cam_Initialized;
                        Camera.AutoFocusCompleted -= AutoFocusCompleted;
                        Camera.CaptureCompleted -= CaptureCompleted;
                        Camera.CaptureImageAvailable -= CaptureImageAvailable;
                        //Camera.Dispose();
                        Camera = null;
                        cameraIsInitialized = false;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception) { }
        }

        public void ListenTap()
        {
            this.Tap += Focus;
        }

        private void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            try
            {
                if (Camera != null)
                {
                    resolution = Camera.AvailableResolutions.Last();
                    Camera.Resolution = resolution;
                    if (Initialized != null)
                    {
                        Initialized(this, null);
                    }
                    cameraIsInitialized = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Capture()
        {
            try
            {
                Camera.CaptureImage();
                if (!mute)
                    sc.PlaySound("Assets/Audio/shoot.wav");
                else
                    viber();
            }
            catch (InvalidOperationException ioe)
            {
                Camera.Initialized += (s, e) =>
                {
                    Capture();
                };
                Capture();
            }
        }

        private void viber()
        {
            VibrateController vc = VibrateController.Default;
            vc.Start(TimeSpan.FromMilliseconds(100));
        }

        public void Focus()
        {
            if (!busy && cameraIsInitialized)
            {
                busy = true;
                if (Camera.IsFocusSupported)
                {
                    try
                    {
                        Camera.Focus();
                    }
                    catch (InvalidOperationException iose)
                    {
                        Focus();
                    }
                }
                else
                    AutoFocusCompleted(null, null);

                busy = false;
            }
        }
        private void Focus(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (!busy && cameraIsInitialized)
            {
                busy = true;
                this.Tap += Focus;
                if (Camera.IsFocusAtPointSupported)
                {
                    try
                    {
                        if (!mute)
                            sc.PlaySound("Assets/Audio/focus.wav");
                        else
                        {
                            viber();
                        }
                        Point pt = e.GetPosition((UIElement)sender);
                        float X = MathUtil.Norm((float)pt.X, (float)0, (float)this.Width);
                        float Y = MathUtil.Norm((float)pt.Y, (float)0, (float)this.Height);
                        Debugger.Log(0, "", X + " " + Y);
                        Camera.FocusAtPoint(X, Y);
                    }
                    catch (Exception)
                    {
                        busy = false;
                        Focus();
                    }
                }
                else
                {
                    busy = false;
                    Focus();
                }
            }
        }
        private void CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            if (cameraIsInitialized)
            {
                Dispatcher.BeginInvoke(() => ThreadSafeImageCapture(e));
            }
        }

        void ThreadSafeImageCapture(ContentReadyEventArgs e)
        {
            busy = false;
            WriteableBitmap image = BitmapFactory.New((int)Camera.AvailableResolutions.LastOrDefault().Width,
                (int)Camera.AvailableResolutions.LastOrDefault().Height);
            image.SetSource(e.ImageStream);
            if (Type == CameraType.FrontFacing)
            {
                image = WriteableBitmapExtensions.Flip(image, WriteableBitmapExtensions.FlipMode.Vertical);
                //image = WriteableBitmapExtensions.Flip(image, WriteableBitmapExtensions.FlipMode.Horizontal);
            }
            image = WriteableBitmapExtensions.Rotate(image, (int)angle);
            OrientedCameraImage orientedImage = new OrientedCameraImage()
            {
                Image = image,
                Angle = angle,
                Orientation = orientation
            };
            if (Captured != null)
            {
                Captured(this, orientedImage);
            }
        }

        private void CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
        }

        private void AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            Capture();
        }

        public void UpdateOrientation()
        {
            switch (orientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                    angle = 0;
                    WidthToLandscape();
                    break;
                case PageOrientation.LandscapeRight:
                    angle = 180;
                    WidthToLandscape();
                    break;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitUp:
                    angle = 90;
                    WidthToPortrait();
                    break;
                case PageOrientation.PortraitDown:
                    angle = -90;
                    WidthToPortrait();
                    break;
            }
            videoBrushTransform.Rotation = angle;
        }

        private void WidthToLandscape()
        {
            Width = InitWidth;
            Height = 0.75 * Width;
        }

        private void WidthToPortrait()
        {
            Height = InitWidth;
            Width = 0.75 * Height;
        }

        public void StopListenTap()
        {
            this.Tap -= Focus;
        }
    }
}
