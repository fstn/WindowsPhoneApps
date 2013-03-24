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
using TweetMaps.Resources;
using Windows.Phone.Media.Capture;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using FstnUserControl.Error;
using FstnUserControl;
using Microsoft.Expression.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Maps.Controls;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;

namespace TweetMaps
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
        }


        private void InitAppBar()
        {
            var theme = ThemeManager.Instance.Theme;
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Assets/Images/" + theme + "/appbar.share.png", Msg.Save, AskToShare);
        }

        private void getTweet()
        {
            WebClient wc=new WebClient();
            wc.DownloadStringCompleted+=wc_DownloadStringCompleted;
            wc.DownloadStringAsync(new Uri("https://search.twitter.com/search.atom?q=to:twitter%20geocode:37.781157,-122.398720,25mi", UriKind.Absolute));
            
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XDocument doc = XDocument.Parse(e.Result);
            try
            {
                 String defaut= "{http://www.w3.org/2005/Atom}";
                List<Object> listObjects = (from c in doc.Descendants(defaut+"entry")
                              select new Object()
                              {
                                  Id = c.Element("{http://www.w3.org/2005/Atom}id").Value,
                                  Title = c.Element("{http://www.w3.org/2005/Atom}title").Value,
                                  SortTitle = c.Element("{http://schemas.zune.net/catalog/apps/2008/02}sortTitle").Value,
                                  Update = Convert.ToDateTime(c.Element("{http://www.w3.org/2005/Atom}updated").Value),
                                  ReleaseDate = Convert.ToDateTime(c.Element("{http://schemas.zune.net/catalog/apps/2008/02}releaseDate").Value),
                                  Version = c.Element("{http://schemas.zune.net/catalog/apps/2008/02}version").Value,
                                  AverageUserRating = Convert.ToDouble(c.Element("{http://schemas.zune.net/catalog/apps/2008/02}averageUserRating").Value),
                                  UserRatingCount = Convert.ToInt16(c.Element("{http://schemas.zune.net/catalog/apps/2008/02}userRatingCount").Value),
                                  Image = new MarketImage()
                                  {
                                      Id = c.Element("{http://schemas.zune.net/catalog/apps/2008/02}image").Element("{http://schemas.zune.net/catalog/apps/2008/02}id").Value
                                  }
                              }).ToList<Object>();

            }
            catch (NullReferenceException ne)
            {
            }
        }

        private void AskToShare(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}