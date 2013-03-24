using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FstnCommon.Util;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyToolkit.Multimedia;
using MyToolkit.Networking;

namespace FstnUserControl.Video
{
    public partial class YoutubeVideo : UserControl
    {

        public string Title { get; set; }
        public string VideoImageUrl { get; set; }
        public string VideoId { get; set; }
        public string VideoUrl { get; set; }
        public YoutubeVideo()
        {
            InitializeComponent();
            this.Loaded += YoutubeVideo_Loaded;
        }

        void YoutubeVideo_Loaded(object sender, RoutedEventArgs e)
        {
            TitleUI.Text = "Text:" + Title;
            if (VideoImageUrl != null)
            {
                BitmapImage bit=new BitmapImage(new Uri(VideoImageUrl));
                VideoImageUrlUI.Source = bit;
                VideoImageUrlUIReflect.Source = bit;
            }
            LayoutRoot.Tap += LayoutRoot_Tap;
            if(SystemTray.ProgressIndicator!=null)
            SystemTray.ProgressIndicator.IsVisible = false;
        }

        void Instance_Navigated(object sender, EventArgs e)
        {
            Stop();
        }

        void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AppManager.Instance.Navigated += Instance_Navigated;
            if (SystemTray.ProgressIndicator != null)
            SystemTray.ProgressIndicator.IsVisible = true; 
            HttpResponse resp = YouTube.Play(VideoId.Split('/').LastOrDefault(), YouTubeQuality.Quality480P, null);
            Debugger.Log(0, "", VideoId + " " + VideoImageUrl);
        }

        public void Stop()
        {
            AppManager.Instance.Navigated -= Instance_Navigated;
            SystemTray.ProgressIndicator.IsVisible = false; 
            YouTube.CancelPlay();
        }
    }
}
