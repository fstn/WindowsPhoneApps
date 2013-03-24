using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace MarketRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {
        private String theme;
        private Queue<AppsPreview> appsPreviewToLoad;
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
            else
                Background.Source = new BitmapImage(new Uri("Image/" + theme + "/Background.png", UriKind.Relative));

            //set place where message error will be print
            ErrorService.Instance.ErrorDisplayer = (IErrorDisplayer)ErrorDisplayer;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BuildApplicationBar();
            LoadXML();
        }

        //load pivot content when it is enable
        void ContentLayout_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null)
            {
                AppsPreview currentPreview = listOfPreviews[currentTab];
                //currentPreview.load();
            }
        }

        void LoadXML()
        {
            listOfCats = new Dictionary<int, MarketCat>();
            listOfPreviews = new Dictionary<int, AppsPreview>();
            appsPreviewToLoad = new Queue<AppsPreview>();
            // listOfUri = new Dictionary<int, Uri>();
            int i = 0;
            foreach (MarketCat category in MarketCatGenerator.Instance.Categories)
            {
                listOfCats.Add(i, category);
                i++;
            }

            for (i = 0; i < listOfCats.Count; i++)
            {
                AppsPreview appsP = CreateAppsPreview(listOfCats[i]);
                listOfPreviews.Add(i, appsP);
                appsPreviewToLoad.Enqueue(appsP);
                PivotItem pItem = new PivotItem();
                pItem.Margin = new Thickness(0);
                pItem.Padding = new Thickness(0);
                pItem.Content = listOfPreviews[i];
                pItem.Header = listOfCats[i].Title;
                ContentLayout.Items.Add(pItem);
            }
            AppsPreview appP = appsPreviewToLoad.Dequeue();
            appP.load();
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
            RandomAppsPreview.CompletedEvent += RandomAppsPreview_Loaded;
            return RandomAppsPreview;
        }

        void RandomAppsPreview_Loaded(object sender, EventArgs e)
        {
            if (appsPreviewToLoad.Count > 0)
            {
                AppsPreview appP = appsPreviewToLoad.Dequeue();
                appP.load();
                RunPeriodicJob();
            }
            else
            {
                RunPeriodicJob();
            }
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
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.settings.png", Msg.Settings, AskToSettings);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.shuffle.png", Msg.Reload, AskToReload);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
        }

        private void AskToShare(object sender, EventArgs e)
        {
            int currentTab = ContentLayout.SelectedIndex;
            if (listOfPreviews[currentTab] != null && listOfPreviews[currentTab].IsLoaded == true)
            {
                try
                {
                    ShareMediaTask task = new ShareMediaTask();
                    task.FilePath = ScreenShot.Take(listOfPreviews[currentTab].DisplayPart).GetPath();
                    task.Show();
                }
                catch (Exception ex)
                {
                    Debugger.Log(0, "", ex.Message);
                }

            }
        }

        private void AskToSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
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
        {
            Uri tileUri = new Uri(string.Concat("/MainPage.xaml?", "tile=cycle"), UriKind.Relative);
            var i = 0;
            List<String> imageNames = new List<string>();
            foreach (AppsPreview prev in listOfPreviews.Values)
            {
                if (prev.MarketApp != null)
                {
                    var name = ScreenShot.TakeInIsolatedSotrage(prev.ImageDisplayPart, "image" + i + ".jpg");
                    if (name != null)
                    {
                        imageNames.Add(name);
                        i++;
                    }
                }
            }

            CycleTileData cycleTileData = new CycleTileData();
            cycleTileData.Title = "";
            cycleTileData.Count = i;
            cycleTileData.SmallBackgroundImage = new Uri("/Image/App Roulette.png", UriKind.Relative);
            
            cycleTileData.CycleImages = imageNames.Select(
            imageName => new Uri(imageName, UriKind.Absolute));
            ShellTile tile = ShellTile.ActiveTiles.First();
             if (tile!= null)
             {
                 tile.Update(cycleTileData);
             }
        }
    }
}