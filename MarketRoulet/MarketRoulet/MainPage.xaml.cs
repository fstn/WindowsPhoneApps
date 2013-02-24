using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FstnCommon.Loader;
using FstnCommon.Parser;
using FstnUserControl;
using FstnUserControl.Apps.Loader;
using FstnUserControl.Apps.Model;
using MarketRoulet.Resources;
using MarketRoulet.Util;
using MarketRoulet.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MarketRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {

        private String theme;
        private Dictionary<int, AppsPreview> listOfPreviews;
        private Dictionary<int, Uri> listOfUri;
        private Dictionary<int, MarketCat> listOfCats;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            
            ContentLayout.ManipulationCompleted += ContentLayout_ManipulationCompleted;
            theme = ""; //field level var (could make it dark by default if needed)
            if ((Visibility)App.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
            {
                theme = "dark";
            }
            else
            {
                theme = "light";
            }
            AdControl.ErrorOccurred += AdControl_ErrorOccurred;

        }

        void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debugger.Log(0, "info", e.Error.Message);
        }

        void ContentLayout_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                currentPreview.load();
                MarketCatTitle.Text = Msg.Title + ": " + listOfCats[currentTab].Title;
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BuildApplicationBar();
            LoadXML();
            RunPeriodicJob();
        }


        void LoadXML()
        {
            listOfCats = new Dictionary<int, MarketCat>();
            // listOfUri = new Dictionary<int, Uri>();
            listOfCats.Add(0, new MarketCat("windowsphone.Best", "Top"));
            listOfCats.Add(1, new MarketCat("windowsphone.Games", "Games"));
            listOfCats.Add(2, new MarketCat("windowsphone.MusicAndVideo", "Music Video"));
            listOfCats.Add(3, new MarketCat("windowsphone.NewsAndWeather", "News"));
            listOfCats.Add(4, new MarketCat("windowsphone.Photo", "Photo"));
            listOfCats.Add(5, new MarketCat("windowsphone.Social", "Social"));
            listOfCats.Add(6, new MarketCat("windowsphone.Sports", "Sports"));
            listOfCats.Add(7, new MarketCat("windowsphone.ToolsAndProductivity", "Tools"));
            listOfPreviews = new Dictionary<int, AppsPreview>();
            for (int i = 0; i < listOfCats.Count; i++)
            {
                Uri uri = URIModel.Instance.getRandomWithCat(listOfCats[i]);
                listOfPreviews.Add(i, CreateAppsPreview(uri));
                PivotItem pItem = new PivotItem();
                pItem.Margin = new Thickness(0);
                pItem.Padding = new Thickness(0);
                pItem.Content = listOfPreviews[i];
                ContentLayout.Items.Add(pItem);
            }
            listOfPreviews[0].load();
        }

        private AppsPreview CreateAppsPreview(Uri previewUri)
        {
            XMLLoader loader = new XMLLoader();
            XMLParser parser = new MarketAppXMLParserFromMP();
            XMLLoader previewLoader = new XMLLoader();
            XMLParser previewParser = new MarketAppXMLParser();
            Uri uri = URIModel.Instance.getBaseAppsUri();
            AppsPreview RandomAppsPreview;
            RandomAppsPreview = new AppsPreview();
            RandomAppsPreview.PreviewLoader = previewLoader;
            RandomAppsPreview.Loader = loader;
            RandomAppsPreview.PreviewParser = previewParser;
            RandomAppsPreview.Parser = parser;
            RandomAppsPreview.PreviewURI = previewUri;
            RandomAppsPreview.URI = uri;
            RandomAppsPreview.ErrorEvent += RandomAppsPreview_ErrorEvent;
            return RandomAppsPreview;
        }
        void RandomAppsPreview_ErrorEvent(object sender, object obj)
        {
            var rootFrame = (App.Current as App).RootFrame;
            rootFrame.Navigate(new System.Uri("/FstnDesign;component/Error.xaml", System.UriKind.Relative));
        }

        private void RemoveRandomApps()
        {
            //ContentLayout.Children.Clear();
            //RandomAppsPreview = null;
        }
        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];
            var appBarButtonAdd = new ApplicationBarIconButton(new Uri("/image/" + theme + "/appbar.cards.heart.png", UriKind.Relative)) { Text = Msg.Rate };
            appBarButtonAdd.Click += AskToRate;
            ApplicationBar.Buttons.Add(appBarButtonAdd);

            var appBarMenuReview = new ApplicationBarMenuItem(Msg.Rate);
            appBarMenuReview.Click += AskToRate;
            ApplicationBar.MenuItems.Add(appBarMenuReview);

            var appBarButtonAddTile = new ApplicationBarIconButton(new Uri("/image/" + theme + "/appbar.pin.png", UriKind.Relative)) { Text = Msg.AddTile };
            appBarButtonAddTile.Click += AskToAddTile;
            ApplicationBar.Buttons.Add(appBarButtonAddTile);

            var appBarMenuAddTile = new ApplicationBarMenuItem(Msg.AddTile);
            appBarMenuAddTile.Click += AskToAddTile;
            ApplicationBar.MenuItems.Add(appBarMenuAddTile);

            var appBarButtonReload = new ApplicationBarIconButton(new Uri("/image/" + theme + "/appbar.shuffle.png", UriKind.Relative)) { Text = Msg.Reload };
            appBarButtonReload.Click += AskToReload;
            ApplicationBar.Buttons.Add(appBarButtonReload);

            var appBarMenuReload = new ApplicationBarMenuItem(Msg.Reload);
            appBarMenuReload.Click += AskToReload;
            ApplicationBar.MenuItems.Add(appBarMenuReload);
        }

        private void AskToAddTile(object sender, EventArgs e)
        {
           

            // get application tile
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                MarketApp currentApp = currentPreview.MarketApp;
                // create a new data for tile
                StandardTileData data = new StandardTileData();
                // tile foreground data
                data.Title = Msg.Title;

                if (currentApp != null)
                {
                    data.BackgroundImage = new Uri(currentApp.Image, UriKind.Absolute);
                }
                // to make tile flip add data to background also
                data.BackTitle = Msg.Title;
                data.BackBackgroundImage = new Uri(currentApp.Image, UriKind.Absolute);
                data.BackContent = currentApp.Title;
                ShellTile.Create(new Uri("/MainPage.xaml?MarketRouletTile=1&categorie="+listOfCats[currentTab].Id+"&applicationId=" + currentApp.Id, UriKind.Relative), data);
               
                //use to communicate with Tile
                var settings = IsolatedStorageSettings.ApplicationSettings;
                settings[listOfCats[currentTab].Id] = listOfPreviews[currentTab].MarketApp.Id;
                settings.Save();

            }
        }


        private void RunPeriodicJob()
        {  //start background agent 
            ResourceIntensiveTask periodicTask = new ResourceIntensiveTask("PeriodicAgent");
            periodicTask.Description = "My live tile periodic task";
            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove("PeriodicAgent");
            }
            //only can be called when application is running in foreground
            ScheduledActionService.Add(periodicTask);
            ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(10));
        }

        private void AskToRate(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
        private void AskToReload(object sender, EventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                currentPreview.Reload();
            }
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string categorieId = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("categorie", out categorieId))
            {
                NavigationContext.QueryString.Remove("categorie");
                var settings = IsolatedStorageSettings.ApplicationSettings;
                if (settings.Contains(categorieId))
                {
                    MarketplaceDetailTask market = new MarketplaceDetailTask();
                    market.ContentIdentifier = (String)settings[categorieId];
                    market.Show();
                }
            }
        }
    }
}