using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using FstnUserControl.Apps.Loader;
using FstnUserControl.Apps.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace UpdateMarketRouletTile
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        private ShellTile currentTile;
        private WebClient wc;
        private List<ShellTile> TilesToFind;
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Debugger.Log(3, "warning", e.ExceptionObject.ToString());
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        { //TODO: Add code to perform your task in background

            wc = new WebClient();
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
            /// If application uses both PeriodicTask and ResourceIntensiveTask
            if (task is ResourceIntensiveTask)
            {
                // Execute periodic task actions here.
                TilesToFind = ShellTile.ActiveTiles.Where(x => x.NavigationUri.ToString().Contains("MarketRouletTile")).ToList();
                runNewDownload();
            }
            else
            {
                // Execute resource-intensive task actions here.
            }
        }


        void runNewDownload()
        {
            if (TilesToFind.Count > 0)
            {
                currentTile = TilesToFind.ElementAt(0);

                wc.DownloadStringAsync(AgentURIModel.Instance.getRandomWithCat(GetCategorie(currentTile.NavigationUri.OriginalString)));
                TilesToFind.RemoveAt(0);
            }
            else
            {
                 NotifyComplete();
            }
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            MarketAppXMLParser parser = new MarketAppXMLParser();
            XDocument doc = XDocument.Parse(e.Result);
            parser.parse(doc);
            MarketApp app = parser.ParsedApp;
            StandardTileData NewTileData = new StandardTileData
            {
                Title = app.Title,
                BackgroundImage = new Uri(app.Image, UriKind.Absolute),
                BackBackgroundImage = new Uri(app.Image, UriKind.Absolute),
                BackContent = app.Title
            };
            currentTile.Update(NewTileData);
            //use to communique with main app
            var settings = IsolatedStorageSettings.ApplicationSettings;
            String currentCat=GetCategorie(currentTile.NavigationUri.OriginalString);
            if (settings.Contains(currentCat))
                settings[currentCat] = app.Id;
            settings.Save();
        }

         public String GetCategorie(string url) {
             int queryStartIndex = url.IndexOf("&categorie=") + 11;
             int queryEndIndex = url.IndexOf("&applicationId") ;
             return url.Substring( queryStartIndex,queryEndIndex - queryStartIndex);
         }
    }

}