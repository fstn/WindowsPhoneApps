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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FstnCommon.Loader;
using FstnCommon.Market.Category;
using FstnCommon.Market.Model;
using FstnCommon.Parser;
using FstnCommon.Util;
using FstnCommon.Util.Settings;
using FstnDesign.FstnColor;
using FstnUserControl;
using FstnUserControl.ApplicationBar;
using FstnUserControl.Error;
using FstnUserControl.Market.Loader;
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
        private Dictionary<int, MarketCat> listOfCats;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            ContentLayout.ManipulationCompleted += ContentLayout_ManipulationCompleted;

            theme = ThemeManager.Instance.Theme;
            if (theme == "light")
                Background.Source = new BitmapImage(new Uri("Image/" + theme + "/Background.png", UriKind.Relative));

            //set place where message error will be print
            ErrorService.Instance.ErrorDisplayer = (IErrorDisplayer)ErrorDisplayer;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BuildApplicationBar();
            LoadXML();
            RunPeriodicJob();
        }

        //load pivot content when it is enable
        void ContentLayout_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                currentPreview.load();
            }
        }

        void LoadXML()
        {
            listOfCats = new Dictionary<int, MarketCat>();
            listOfPreviews = new Dictionary<int, AppsPreview>();
            // listOfUri = new Dictionary<int, Uri>();
            int i = 0;
            foreach (MarketCat category in MarketCatGenerator.Instance.Categories)
            {
                listOfCats.Add(i, category);
                i++;
            }

            for (i = 0; i < listOfCats.Count; i++)
            {
                listOfPreviews.Add(i, CreateAppsPreview(listOfCats[i]));
                PivotItem pItem = new PivotItem();
                pItem.Margin = new Thickness(0);
                pItem.Padding = new Thickness(0);
                pItem.Content = listOfPreviews[i];
                pItem.Header = listOfCats[i].Title;
                ContentLayout.Items.Add(pItem);
            }
            listOfPreviews[0].load();
        }

        private AppsPreview CreateAppsPreview(MarketCat Cat)
        {
            XMLLoader loader = new XMLLoader();
            XMLParser parser = new MarketAppXMLParserFromMP();
            XMLLoader previewLoader = new XMLLoader();
            XMLParser previewParser = new MarketAppXMLParser();
            Uri uri = URIModel.Instance.getBaseAppsUri();
            AppsPreview RandomAppsPreview;
            RandomAppsPreview = new AppsPreview();
            RandomAppsPreview.URI = uri;
            RandomAppsPreview.Category = Cat;
            RandomAppsPreview.PreviewLoader = previewLoader;
            RandomAppsPreview.Loader = loader;
            RandomAppsPreview.PreviewParser = previewParser;
            RandomAppsPreview.Parser = parser;
            RandomAppsPreview.UrlGetter = URIModel.Instance.getRandomWithCat;
            RandomAppsPreview.ErrorEvent += RandomAppsPreview_ErrorEvent;
            return RandomAppsPreview;
        }
        void RandomAppsPreview_ErrorEvent(object sender, object obj)
        {
            ErrorService.Instance.AddError(this, "preview error", ErrorType.EMPTY_RESPONSE_FROM_SERVER);
        }
        private void BuildApplicationBar()
        {

            ApplicationBar = new ApplicationBar();
            ApplicationBar.BackgroundColor = (Color)App.Current.Resources["PhoneAccentColor"];
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.cards.heart.png", Msg.Rate, AskToRate);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.pin.png", Msg.AddTile, AskToAddTile);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.settings.png", Msg.Settings, AskToSettings);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.shuffle.png", Msg.Reload, AskToReload);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
        }

        private void AskToShare(object sender, EventArgs e)
        {/*
            ShareMediaTask task = new ShareMediaTask();
            task.FilePath = ScreenShot.Take(LayoutRoot).GetPath();
            task.Show();*/
        }

        private void AskToSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void AskToAddTile(object sender, EventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                MarketApp currentApp = currentPreview.MarketApp;

                StandardTileData data = new StandardTileData();
                data.Title = Msg.Title;

                if (currentApp != null)
                {
                    data.BackgroundImage = new Uri(currentApp.Image, UriKind.Absolute);
                }
                data.BackTitle = Msg.Title;
                data.BackBackgroundImage = new Uri(currentApp.Image, UriKind.Absolute);
                data.BackContent = currentApp.Title;
                List<ShellTile> TilesToFind = ShellTile.ActiveTiles.Where(x => x.NavigationUri.ToString().Contains("categorie=" + listOfCats[currentTab].Id)).ToList();
                foreach (ShellTile tile in TilesToFind)
                {
                    tile.Delete();
                }

                ShellTile.Create(new Uri("/MainPage.xaml?MarketRouletTile=1&categorie=" + listOfCats[currentTab].Id + "&applicationId=" + currentApp.Id, UriKind.Relative), data);

                SettingsService.Instance.Save(listOfCats[currentTab].Id, listOfPreviews[currentTab].MarketApp.Id);
            }
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

        //use to redirect to market if user come from tile
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string categorieId = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("categorie", out categorieId))
            {
                NavigationContext.QueryString.Remove("categorie");
                String appToShowInMarket = SettingsService.Instance.Value<String>(categorieId);
                if (appToShowInMarket != null)
                {
                    MarketplaceDetailTask market = new MarketplaceDetailTask();
                    market.ContentIdentifier = appToShowInMarket;
                    market.Show();
                }
            }
        }


        private void RunPeriodicJob()
        {  //start background agent 
            PeriodicTask periodicTask = new PeriodicTask("PeriodicAgent");
            periodicTask.Description = "My live tile periodic task";
            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove("PeriodicAgent");
            }
            //only can be called when application is running in foreground
            ScheduledActionService.Add(periodicTask);
            // ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(10));
        }

    }
}