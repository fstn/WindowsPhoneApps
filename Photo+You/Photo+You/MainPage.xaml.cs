using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using FstnCommon.Util;
using FstnUserControl.Camera;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Photo_You.Resources;
using Windows.Phone.Media.Capture;

namespace Photo_You
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PhotoCamera PrimaryCamera;
        private PhotoCamera SecondCamera;
        private CameraShooter secondShooter;
        private CameraShooter primaryShooter;
        private Canvas Content;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Content=new Canvas();
            Content.Background=new SolidColorBrush(Colors.Cyan);
            capture.Content=Content;

            primaryShooter = new CameraShooter(CameraType.Primary);
            Content.Children.Add(primaryShooter);
            primaryShooter.Width = 820;
            primaryShooter.Height = 480;
            primaryShooter.ListenTap();
            primaryShooter.Captured += primaryShooter_Captured;
            primaryShooter.Start();
            secondShooter = new CameraShooter(CameraType.FrontFacing);
            secondShooter.Captured += secondShooter_Captured;
            Content.Children.Add(secondShooter);
            backgroundPhoto.Tap += MovePhotographerTo;
        }

        void MovePhotographerTo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Canvas.SetLeft(photographPhoto, e.GetPosition(backgroundPhoto).X - photographPhoto.Width/2);
            Canvas.SetTop(photographPhoto, e.GetPosition(backgroundPhoto).Y - photographPhoto.Height / 2);
        }


        private void secondShooter_Captured(object sender, System.Windows.Media.Imaging.BitmapImage e)
        {
            ImageBrush still = new ImageBrush();
            still.ImageSource = e;
            photographPhoto.Fill = still;
            primaryShooter.Start();
        }

        void primaryShooter_Captured(object sender, System.Windows.Media.Imaging.BitmapImage e)
        {
            ImageBrush still = new ImageBrush();
            still.ImageSource = e; 
            backgroundPhoto.Fill = still;
            secondShooter.StartAndShoot();
        }
        
    }
}