using System;
using System.Collections.Generic;
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
using MarketRoulet.Resources;
using MarketRoulet.Util;
using MarketRoulet.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MarketRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {

        private String theme;
        private AppsPreview RandomAppsPreview;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            theme = ""; //field level var (could make it dark by default if needed)
            if ((Visibility)App.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
            {
                theme = "dark";
            }
            else
            {
                theme = "light";
            }

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BuildApplicationBar();
            LoadXML();
        }

        void LoadXML()
        {
            XMLLoader loader = new XMLLoader();
            XMLParser parser = new MarketAppXMLParserFromMP();
            XMLLoader previewLoader = new XMLLoader();
            XMLParser previewParser = new MarketAppXMLParser();
            Uri previewUri = URIModel.Instance.getFstnRandomBestApp();
            Uri uri = URIModel.Instance.getBaseAppsUri();
            if (RandomAppsPreview != null)
                RemoveRandomApps();
            RandomAppsPreview = new AppsPreview();
            RandomAppsPreview.PreviewLoader = previewLoader;
            RandomAppsPreview.Loader = loader;
            RandomAppsPreview.PreviewParser = previewParser;
            RandomAppsPreview.Parser = parser;
            RandomAppsPreview.PreviewURI = previewUri;
            RandomAppsPreview.URI = uri;
            RandomAppsPreview.load();
            ContentLayout.Children.Add(RandomAppsPreview);
        }

        private void RemoveRandomApps()
        {
            ContentLayout.Children.Clear();
            RandomAppsPreview = null;
        }
        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var appBarButtonAdd = new ApplicationBarIconButton(new Uri("/image/" + theme + "/appbar.cards.heart.png", UriKind.Relative)) { Text = Msg.Rate };
            appBarButtonAdd.Click += AskToRate;
            ApplicationBar.Buttons.Add(appBarButtonAdd);

            var appBarMenuReview = new ApplicationBarMenuItem(Msg.Rate);
            appBarMenuReview.Click += AskToRate;
            ApplicationBar.MenuItems.Add(appBarMenuReview);


            var appBarButtonReload = new ApplicationBarIconButton(new Uri("/image/" + theme + "/appbar.shuffle.png", UriKind.Relative)) { Text = Msg.Reload };
            appBarButtonReload.Click += AskToReload;
            ApplicationBar.Buttons.Add(appBarButtonReload);

            var appBarMenuReload = new ApplicationBarMenuItem(Msg.Reload);
            appBarMenuReload.Click += AskToReload;
            ApplicationBar.MenuItems.Add(appBarMenuReload);
        }
        private void AskToRate(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
        private void AskToReload(object sender, EventArgs e)
        {
            Uri previewUri = URIModel.Instance.getFstnRandomBestApp();
            RandomAppsPreview.Reload(previewUri);
        }
    }
}