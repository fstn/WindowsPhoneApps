using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using MarketRoulet.Resources;
using FstnUserControl;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using FstnCommon;
using System.Net;
using MarketRoulet.Util;
using System.Xml.Linq;
using FstnUserControl.Apps.Model;
using FstnCommon.Loader;
using FstnCommon.Parser;
using FstnUserControl.Apps.Loader;
namespace MarketRoulet.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region prop
        #endregion

        private MarketApp defaultRandomApp;

        public MarketApp DefaultRandomApp
        {
            get {
                return defaultRandomApp; }
            set { defaultRandomApp = value; }
        }
        
        public MainViewModel()
        {
            
            
           /* LoadDefaultRandomApp();
            LoadRandomApp();
            LoadRandomGame();*/
                    
        }
        /*
        private void LoadRandomApp(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXmlns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
           RandomApp = LoadEntityFromXml(doc);
           OnPropertyChanged("RandomApp");
                          }

        private void LoadRandomGame(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXmlns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            RandomGame = LoadEntityFromXml(doc);
            OnPropertyChanged("RandomGame");
        }
        */
        private void LoadDefaultRandomApp(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXmlns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            defaultRandomApp=LoadEntityFromXml(doc);
            OnPropertyChanged("DefaultRandomApp");
        }

        private MarketApp LoadEntityFromXml(XDocument doc)
        {
            MarketApp retour = new MarketApp(); 
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            retour = new MarketApp()
            {
                Id = doc.Element(atom + "entry").Element(atom + "id").Value,
                Title = doc.Element(atom + "entry").Element(atom + "title").Value,
                SortTitle = doc.Element(atom + "entry").Element(defaultXlmns + "sortTitle").Value,
                Image = doc.Element(atom + "entry").Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value
            };
            retour.Id = retour.Id.Replace("urn:uuid:", String.Empty);
            retour.Image = retour.Image.Replace("urn:uuid:", String.Empty);
            retour.Image = URIModel.Instance.getImageUriString() + retour.Image;
            return retour;
        }



/*
        internal void LoadRandom()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += LoadDefaultRandomApp;
            client.DownloadStringAsync(URIModel.Instance.getRandomAllURI());        
        }

        internal void LoadRandomApp()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += LoadRandomApp;
            client.DownloadStringAsync(URIModel.Instance.getRandomAppURI());        
        }

        internal void LoadRandomGame()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += LoadRandomGame;
            client.DownloadStringAsync(URIModel.getInstance().getRandomGameURI());        
        }*/
    }
}
