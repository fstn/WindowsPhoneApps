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
using GiveMeSomething.Resources;
using GiveMeSomething.Util;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;

namespace GiveMeSomething
{
    public partial class MainPage : PhoneApplicationPage
    {
        private String theme;
        private List<MarketCat> listOfCats;
        private SpeechSynthesizer ss;
        private SpeechRecognizerUI recoWithUI;
        private bool busy = false;
        private AppsPreview chooseAppsPreview;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            theme = ThemeManager.Instance.Theme;
            if (theme == "light")
                Background.Source = new BitmapImage(new Uri("Image/" + theme + "/Background.png", UriKind.Relative));
            else
                Background.Source = new BitmapImage(new Uri("Image/" + theme + "/Background.png", UriKind.Relative));

            //set place where message error will be print
            ErrorService.Instance.ErrorDisplayer = (IErrorDisplayer)ErrorDisplayer;
            ss = new SpeechSynthesizer();
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            BuildApplicationBar();
            LoadXML();
            Listen();
        }


        void LoadXML()
        {
            listOfCats = new List<MarketCat>();
            int i = 0;
            foreach (MarketCat category in MarketCatGenerator.Instance.Categories)
            {
                listOfCats.Add(category);
                i++;
            }

            /*for (i = 0; i < listOfCats.Count; i++)
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
            }*/
        }
        private async void Listen()
        {
            if (!busy)
            {
                busy = true;
                // Initialize the SpeechRecognizerUI object.
                recoWithUI = new SpeechRecognizerUI();

                // Query for a recognizer that recognizes French as spoken in France.
                IEnumerable<SpeechRecognizerInformation> recognizers = from recognizerInfo in InstalledSpeechRecognizers.All
                                                                       select recognizerInfo;

                // Set the recognizer to the top entry in the query result.
                recoWithUI.Recognizer.SetRecognizer(recognizers.ElementAt(0));

                // Create a string array of French numbers.
                string[] settings = (from MarketCat cat in listOfCats.ToList()
                                     select cat.Title).ToArray();

                // Create a list grammar from the string array and add it to the grammar set.
                recoWithUI.Recognizer.Grammars.AddGrammarFromList("categories", settings);

                string listenText = "";
                string exampleText = "";
                for (int i = 0; i < settings.Length; i++)
                {
                    listenText += settings[i];
                    exampleText += " " + settings[i];
                }
                // Display text to prompt the user's input.
                recoWithUI.Settings.ListenText = "Category Between: ";

                recoWithUI.Settings.ReadoutEnabled = false;
                // Give an example of ideal speech input.
                recoWithUI.Settings.ExampleText = exampleText;

                // Load the grammar set and start recognition.
                SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();
                string action = recoResult.RecognitionResult.Text;
                SpeechSynthesizer ss = new SpeechSynthesizer();
                VoiceInformation vi = InstalledVoices.All.Where(v => v.Language == "en-EN" && v.Gender == VoiceGender.Male).FirstOrDefault();
                ss.SetVoice(vi);
                await ss.SpeakTextAsync("I'm looking for " + action + "!");
                MarketCat chooseCat = (from MarketCat cat in listOfCats.ToList()
                                       where cat.Title == action
                                       select cat).FirstOrDefault();
                chooseAppsPreview = CreateAppsPreview(chooseCat);
                CategorieTitle.Text = chooseCat.Title;
                AppsPreviewContent.Children.Clear();
                AppsPreviewContent.Children.Add(chooseAppsPreview);
                chooseAppsPreview.load();

                chooseAppsPreview.CompletedEvent+= (e, o) =>
                {
                    sayAppName();
                };
               
            }
            busy = false;
        }

        private async void sayAppName()
        {
                await ss.SpeakTextAsync("Try " + chooseAppsPreview.MarketApp.Title + "!");
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
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.microphone.png", Msg.Reload, AskToReload);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/image/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
        }

        private void AskToShare(object sender, EventArgs e)
        {
            if (chooseAppsPreview != null)
            {
                ShareMediaTask task = new ShareMediaTask();
                task.FilePath = ScreenShot.Take(chooseAppsPreview.DisplayPart).GetPath();
                task.Show();
            }
        }

        private void AskToRate(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void AskToReload(object sender, EventArgs e)
        {
            Listen();
        }

        //use to redirect to market if user come from tile
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}