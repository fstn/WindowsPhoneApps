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



namespace FstnUserControl.Camera
{
    public class CameraShooter : Canvas
    {
        public event InitializedEventHandler Initialized;
        public event CaptureEventHandler Captured;
        public PhotoCamera Camera { get; set; }
        private VideoBrush brush;
        private CompositeTransform videoBrushTransform { get; set; }
        public CameraType Type{get;set;}
        private DispatcherTimer dt;
        private TextBlock decountText;
        private int decountNumber = 3;
        private SoundController sc;
        private Size resolution;
        private bool busy = false;
        private PageOrientation orientation = PageOrientation.LandscapeLeft;
        private double angle = 0;
        private Binding HeightBinding;
        private Binding WidthBinding;
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
                if (Type == CameraType.FrontFacing)
                {
                   if(orientation==PageOrientation.PortraitDown)
                       orientation=PageOrientation.PortraitUp;

                   if (orientation == PageOrientation.PortraitUp)
                       orientation = PageOrientation.PortraitDown;

                   if (orientation == PageOrientation.LandscapeLeft)
                       orientation = PageOrientation.LandscapeRight;

                   if (orientation == PageOrientation.LandscapeRight)
                       orientation = PageOrientation.LandscapeLeft;
                }
                UpdateOrientation();
            }
        }
        public CameraShooter(CameraType type)
        {
            if (PhotoCamera.IsCameraTypeSupported(type) == true)
            {
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

        private void InitDT()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(0.8);
            dt.Tick += (s, e) =>
            {
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
                    //Camera.Initialized += ShootAfterInit;
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
            decountText.Text = decountNumber.ToString();
            dt.Start();
            Start();
        }

        public void Start()
        {
            Camera = new PhotoCamera(Type);
            Camera.Initialized += cam_Initialized;
            Camera.AutoFocusCompleted += AutoFocusCompleted;
            Camera.CaptureCompleted += CaptureCompleted;
            Camera.CaptureImageAvailable += CaptureImageAvailable;
            brush.SetSource(Camera);
        }

        public void Stop()
        {
            if (dt != null)
                dt.Stop();
            if (Camera != null)
            {
                Camera.Initialized -= cam_Initialized;
                Camera.AutoFocusCompleted -= AutoFocusCompleted;
                Camera.CaptureCompleted -= CaptureCompleted;
                Camera.CaptureImageAvailable -= CaptureImageAvailable;
                Camera.Dispose();
                Camera = null;
            }
        }

        public void ListenTap()
        {
            this.Tap += Focus;
        }

        private void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            resolution = Camera.AvailableResolutions.Last();
            Camera.Resolution = resolution;
            if (Initialized != null)
            {
                Initialized(this, null);
            }
        }

        public void Capture()
        {
            try
            {
                Camera.CaptureImage();
                sc.PlaySound("Assets/Audio/shoot.wav");
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

        public void Focus()
        {
            if (!busy)
            {
                busy = true;
                if (Camera.IsFocusSupported)
                    Camera.Focus();
                else
                    AutoFocusCompleted(null, null);

            }
        }
        private void Focus(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (!busy)
            {
                busy = true;
                this.Tap += Focus;
                if (Camera.IsFocusAtPointSupported)
                {
                    try
                    {
                        sc.PlaySound("Assets/Audio/focus.wav");
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
            Dispatcher.BeginInvoke(() => ThreadSafeImageCapture(e));
        }

        void ThreadSafeImageCapture(ContentReadyEventArgs e)
        {
            busy = false;
            BitmapImage image = new BitmapImage();
            image.SetSource(e.ImageStream);
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
            //Debugger.Log(0, "", "actual to " + ActualWidth + "x" + ActualHeight);
            Width = InitWidth;
            Height = 0.75 * Width;
            //Debugger.Log(0, "", "resize to " + Width + "x" + Height+"\n");
        }

        private void WidthToPortrait()
        {
            // Debugger.Log(0, "", "init to " + InitWidth + "x" + InitHeight);
            //Debugger.Log(0, "", "actual to " + ActualWidth + "x" + ActualHeight);
            Height = InitWidth;
            Width = 0.75 * Height;
            // Debugger.Log(0, "", "resize to " + Width + "x" + Height + "\n");
        }

        public void StopListenTap()
        {
            this.Tap -= Focus;
        }
    }
}
