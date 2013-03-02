
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;
using FstnCommon;
using FstnCommon.Loader;
using FstnCommon.Market.Model;
using FstnCommon.Parser;
using FstnUserControl.Resources;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

/**
 * AppsPreview
 * User control use to show apps info, it also provide a waiting animation
 **/
namespace FstnUserControl
{
    public delegate Uri UrlGetterDelegate(String categorieId);
    public partial class AppsPreview : UserControl
    {
        public UrlGetterDelegate UrlGetter { get; set; }
        public event LoaderErrorEventHandler ErrorEvent;
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

        private MarketApp marketApp;
        public MarketApp MarketApp
        {
            get { return marketApp; }
            set { marketApp = value; }
        }


        private object tmpObject = null;

        private bool loaded=false;
        #endregion



        public void load()
        {
            if(!loaded){
            #region listeners
            try
            {
                ((XMLParser)previewParser).Completed += AppsPreview_PreviewCompleted;
                ((XMLParser)previewParser).Error += PreviewParserError;                
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
            loaded=true;
            }
        }


        void AppsPreview_Error(object sender, object obj)
        {
            if (ErrorEvent != null)
            {
                ErrorEvent(this, obj);
            }
        }

        public void Reload()
        {
            if (WaitingAnim != null)
            {
                Random random = new Random(DateTime.Now.Millisecond);
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
        void LoadPreloader()
        {
            if (UrlGetter != null)
            {
                previewloader.load(UrlGetter(Category.Id));
            }
        }

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
            marketApp = (MarketApp)obj;
            Uri uriToLoad = new Uri(uri.AbsoluteUri + marketApp.Id);
            loader.load(uriToLoad);
            
        }
        
        //if there is no apps to parse because rating rate is to up
        private void PreviewParserError(object sender, object obj)
        {
            Title.Text = FstnUserControlMsg.NoResultError;
            Description.Text = FstnUserControlMsg.NoResultDescr;
            WaitingAnim.LaunchStopAnimation();
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
                    marketApp = (MarketApp)tmpObject;
                    Title.Text = marketApp.Title;
                    Description.Text = marketApp.Description;
                    Image.Source = new BitmapImage(new Uri(marketApp.Image));
                    RatingControl.Value = marketApp.AverageUserRating / 2;
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
            market.ContentIdentifier = marketApp.Id;
            market.Show();
        }

        public MarketCat Category { get; set; }
    }
}
