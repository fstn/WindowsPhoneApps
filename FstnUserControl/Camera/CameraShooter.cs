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



namespace FstnUserControl.Camera
{
    public class CameraShooter : Canvas
    {
        public event InitializedEventHandler Initialized;
        public event CaptureEventHandler Captured;
        public PhotoCamera Camera { get; set; }
        private VideoBrush brush;
        private CameraType type;
        private DispatcherTimer dt;
        private TextBlock decountText;
        private int decountNumber = 3;
        private SoundController sc;
        public CameraShooter(CameraType type)
        {
            if (PhotoCamera.IsCameraTypeSupported(type) == true)
            {
                this.type = type;
                this.Width = 480;
                this.Height = 240;
                dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromSeconds(1);
                dt.Tick += dt_Tick;
                decountText = new TextBlock();
                decountText.FontSize = 100;
                decountText.TextAlignment = TextAlignment.Center;
                decountText.VerticalAlignment = VerticalAlignment.Center;
                this.Children.Add(decountText);
                sc = new SoundController();
            }
        }
        public void StartAndShoot()
        {
            decountText.Height = Height;
            decountText.Width = Width;
            decountText.Text = decountNumber.ToString();
            dt.Start();
            Start();
        }


        void dt_Tick(object sender, EventArgs e)
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
                Capture();
                dt.Stop();
                decountText.Text = "";
                decountNumber = 3;
            }
            decountNumber--;
        }
        public async void Start()
        {
            Camera = new PhotoCamera(type);
            Camera.Initialized += cam_Initialized;
            Camera.AutoFocusCompleted += AutoFocusCompleted;
            Camera.CaptureCompleted += CaptureCompleted;
            Camera.CaptureImageAvailable +=
                new EventHandler<Microsoft.Devices.ContentReadyEventArgs>
                    (CaptureImageAvailable);
            brush = new VideoBrush();
            this.Background = brush;
            brush.SetSource(Camera);
        }
        public void Stop()
        {
            Camera.Initialized -= cam_Initialized;
            Camera.AutoFocusCompleted -= AutoFocusCompleted;
            Camera.CaptureCompleted -= CaptureCompleted;
            Camera.Dispose();
            Camera = null;
            brush.SetSource((MediaElement)null);
            brush = null;
        }


        public void ListenTap()
        {
            this.Tap += Focus;
        }

        private void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (Initialized != null)
            {
                Initialized(this, null);
            }
        }

        public void Capture()
        {
            sc.PlaySound("Assets/Audio/shoot.wav");
            Camera.CaptureImage();
        }
        public void Focus()
        {
            Camera.Focus();
        }
        private void Focus(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sc.PlaySound("Assets/Audio/focus.wav");
            Point pt = e.GetPosition((UIElement)sender);
            float X = MathUtil.Norm((float)pt.X, (float)0, (float)this.Width);
            float Y = MathUtil.Norm((float)pt.Y, (float)0, (float)this.Height);
            Debugger.Log(0, "", X + " " + Y);
            Camera.FocusAtPoint(X, Y);
        }
        private void CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            Dispatcher.BeginInvoke(() => ThreadSafeImageCapture(e));
        }

        void ThreadSafeImageCapture(ContentReadyEventArgs e)
        {
            BitmapImage image = new BitmapImage();
            image.SetSource(e.ImageStream);
            ImageBrush still = new ImageBrush();
            still.ImageSource = image;
            if (Captured != null)
            {
                Captured(this, image);
            }
        }

        private void CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
        }

        private void AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            Capture();
        }

    }
}
