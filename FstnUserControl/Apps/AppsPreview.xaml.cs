
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FstnCommon;
using FstnCommon.Loader;
using FstnCommon.Parser;
using FstnUserControl.Apps;
using FstnUserControl.Apps.Model;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

/**
 * AppsPreview
 * User control use to show apps info, it also provide a waiting animation
 **/
namespace FstnUserControl
{
    public partial class AppsPreview : UserControl
    {
        public AppsPreview()
        {
            InitializeComponent();
            ContentLayout.Visibility = Visibility.Collapsed;

        }

        #region membres

        private Uri uri;

        public Uri URI
        {
            get { return uri; }
            set { uri = value; }
        }

        private IParser parser;

        public IParser Parser
        {
            get { return parser; }
            set { parser = value; }
        }
        private ILoader loader;

        public ILoader Loader
        {
            get { return loader; }
            set { loader = value; }
        }

        private Uri previewUri;

        public Uri PreviewURI
        {
            get { return previewUri; }
            set { previewUri = value; }
        }
        private IParser previewParser;

        public IParser PreviewParser
        {
            get { return previewParser; }
            set { previewParser = value; }
        }
        private ILoader previewloader;

        public ILoader PreviewLoader
        {
            get { return previewloader; }
            set { previewloader = value; }
        }

        private MarketApp app;
        public MarketApp App
        {
            get { return app; }
            set { app = value; }
        }


        private object tmpObject = null;
        #endregion



        public void load()
        {
            #region listeners
            try
            {
                ((XMLParser)previewParser).Completed += AppsPreview_PreviewCompleted;
            }
            catch (InvalidCastException e)
            {
                Debugger.Log(3, "AppsPreview", "you need to specify a xmlParser");
            }

            
            try
            {
                ((XMLLoader)previewloader).Loaded += AppsPreview_PreviewLoaded;
                ((XMLLoader)previewloader).Error += AppsPreview_Error;
            }
            catch (InvalidCastException e)
            {
                Debugger.Log(3, "AppsPreview", "you need to specify a xmlLoader");
            }
            try
            {
                ((XMLLoader)loader).Loaded += AppsPreview_Loaded;
                ((XMLLoader)loader).Error += AppsPreview_Error;
            }
            catch (InvalidCastException e)
            {
                Debugger.Log(3, "AppsPreview", "expected a MarketApp, get " + loader + " or specify a xmlLoader");
            }
            try
            {
                ((XMLParser)parser).Completed += AppsPreview_Completed;
            }
            catch (InvalidCastException e)
            {
                Debugger.Log(3, "AppsPreview", "you need to specify a xmlParser");
            }
            Image.ImageOpened += Image_Loaded;
            Image.ImageFailed += Image_ImageFailed;
            #endregion
            WaitingAnim.start();
            WaitingAnim.MaskedEvent += WaitingAnim_Masked;
            LoadPreloader();
        }

        void AppsPreview_Error(object sender, object obj)
        {
            throw new NotImplementedException();
        }


        public void Reload(Uri previewUri)
        {
            if (WaitingAnim != null)
            {
                this.previewUri = previewUri;
                Double tempWidth = WaitingAnim.Width;
                Double tempHeigt = WaitingAnim.Height;
                RootLayout.Children.Remove(WaitingAnim);
                WaitingAnim.Clean();
                WaitingAnim = null;
                WaitingAnim = new ImageWaiting();
                WaitingAnim.Height = tempHeigt;
                WaitingAnim.Width = tempWidth;
                RootLayout.Children.Add(WaitingAnim);
                WaitingAnim.MaskedEvent += WaitingAnim_MaskedOnRelaod;
                WaitingAnim.start();
                LoadPreloader();
            }
        }
        void LoadPreloader() { previewloader.load(previewUri); }

        //call where we are sure that background is masked
        void WaitingAnim_Masked()
        {
            ContentLayout.Visibility = Visibility.Visible;
            LoadAppFromObj();
        }

        //call where first app is end
        void WaitingAnim_MaskedOnRelaod()
        {
            ContentLayout.Visibility = Visibility.Visible;
            LoadAppFromObj();
        }


        void AppsPreview_PreviewLoaded(object sender, object obj) { previewParser.parse((XDocument)obj); }

        void AppsPreview_PreviewCompleted(object sender, object obj)
        {
            app = (MarketApp)obj;
            Uri uriToLoad = new Uri(uri.AbsoluteUri + app.Id);
            loader.load(uriToLoad);
            
        }

        void AppsPreview_Loaded(object sender, object obj) { parser.parse((XDocument)obj); }

        void AppsPreview_Completed(object sender, object obj)
        {
            if (obj != null)
                tmpObject = obj;
            if (WaitingAnim.Masked == true)
            {
                LoadAppFromObj();
            }
        }

        void LoadAppFromObj()
        {
            if (tmpObject != null)
            {
                try
                {
                    app = (MarketApp)tmpObject;
                    Title.Text = app.Title;
                    Description.Text = app.Description;
                    Image.Source = new BitmapImage(new Uri(app.Image));
                    RatingControl.Value = app.AverageUserRating / 2;
                    RatingControl.RatingItemCount = 5;
                    this.Tap += AppsPreview_Tap;
                }
                catch (InvalidCastException e)
                {
                    Debugger.Log(3, "AppsPreview", "expected a MarketApp, get " + tmpObject);
                }
                finally
                {
                    tmpObject = null;
                }
            }
        }

        void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //remove animation
            WaitingAnim.LaunchStopAnimation();
        }

        void Image_Loaded(object sender, RoutedEventArgs e)
        {
            //remove animation
            WaitingAnim.LaunchStopAnimation();
        }

        void AppsPreview_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceDetailTask market = new MarketplaceDetailTask();
            market.ContentIdentifier = app.Id;
            market.Show();
        }
    }
}
