using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using AppsRoulet.Model;
using Microsoft.Phone.Controls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AppsRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<MarketApp> listOfApps;
        private MyDataContext dt;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            dt = new MyDataContext(MyDataContext.DBConnectionString);
            if (dt.DatabaseExists())
            {
                dt.DeleteDatabase();
            }

            dt.CreateDatabase();

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri("http://marketplaceedgeservice.windowsphone.com/v3.2/fr-Fr/appCategories/windowsphone.Games/apps/?clientType=WinMobile%207.1&store=Zest&orderby=releaseDate"));
            client.DownloadStringCompleted += client_DownloadStringCompleted;
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XDocument doc = XDocument.Parse(e.Result);
            listOfApps = (from c in doc.Descendants("{http://www.w3.org/2005/Atom}entry")
                          select new MarketApp()
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
                          }).ToList<MarketApp>();
            foreach (MarketApp app in listOfApps)
            {
                dt.MarketApps.InsertOnSubmit(app);
                dt.SubmitChanges();
                Debugger.Log(1, "info", app.toString() + "\n");

            }

            /*  MarketApp application = new MarketApp();
              application.Title = element.Element("{http://www.w3.org/2005/Atom}title").Value;
              Debugger.Log(1, "info", application.toString());*/
            /*
            song.Title = x.Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;
            song.Album = x.Element("{http://schemas.zune.net/catalog/music/2007/10}album").
                Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;
            song.Artist = x.Element("{http://schemas.zune.net/catalog/music/2007/10}primaryArtist").
                Element("{http://schemas.zune.net/catalog/music/2007/10}name").Value;

            song.AlbumID = x.Element("{http://schemas.zune.net/catalog/music/2007/10}album")
                .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0, 9);


            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/album/" + song.AlbumID));
            client.DownloadStringCompleted += (s, e) => HandleDownload(s, e, song);*/


        }
    }
}