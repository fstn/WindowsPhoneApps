using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BrokeThePig.Resources;
using BrokeThePig.Source.AI;
using BrokeThePig.UC;
using FstnCommon.Util;
using FstnCommon.Util.Settings;
using FstnDesign.FstnColor;
using FstnUserControl.ApplicationBar;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media.PhoneExtensions;

namespace BrokeThePig
{
    public partial class MainPage : PhoneApplicationPage
    {
        public AI GameAI { get; set; }
        private String theme;
        private IsolatedStorageFile _isf;
        public MainPage()
        {
            GameAI = AI.Instance;
            AI.Instance.GameEnd += Instance_GameEnd;
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            theme = ThemeManager.Instance.Theme;

        }

        void Instance_GameEnd()
        {
            LayoutRoot.Children.Clear();
            AI.Instance.CurrentNumber.LevelNumber = 0;
            NavigationService.Navigate(new Uri("/BrokeThePig;component/Congratulation.xaml", UriKind.Relative));
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Pig.Exploded += Pig_Exploded;
            AI.Instance.OnFight += Instance_OnFight;
            ApplicationBar = new ApplicationBar();
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Images/" + theme + "/appbar.share.png", Msg.Share, AskToShare);
            ApplicationBarGenerator.Instance.CreateDouble(ApplicationBar, "/Images/" + theme + "/appbar.lock.png", Msg.Lock, AskToLock);

        }

        private async void AskToLock(object sender, EventArgs e)
        {
            var op = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
        }

        private void AskToShare(object sender, EventArgs e)
        {
            try
            {
                ShareMediaTask task = new ShareMediaTask();
                task.FilePath = ScreenShot.Take(LayoutRoot).GetPath();
                task.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry we can't share at the moment");
            }
        }


        private void AskToBuy(object sender, EventArgs e)
        {
           
        }

        void Pig_Exploded()
        {
            PigLayout.Tap += b_Tap;
        }


        void b_Tap(object sender, GestureEventArgs e)
        {
            PigLayout.Tap -= b_Tap;
            Pig.Exploded -= Pig_Exploded;
            double left = Canvas.GetLeft(Pig);
            double top = Canvas.GetTop(Pig);
            PigLayout.Children.Clear();
            Pig = new Pig();
            Canvas.SetLeft(Pig, left);
            Canvas.SetTop(Pig, top);
            Pig.Exploded += Pig_Exploded;
            PigLayout.Children.Add(Pig);
        }

        private void Button_Tap_1(object sender, GestureEventArgs e)
        {
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentCount, 0);
            AI.Instance.CurrentNumber.LevelNumber = 0;

            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentCount, 0);
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentLevel, 0);
            CryptedSettingsService.Instance.Save(SettingsKeys.CurrentMoney, 0);
            AI.Instance.CurrentNumber.Number = 0;
            AI.Instance.CurrentNumber.LevelNumber = 0;
            AI.Instance.CurrentMoney = 0;
        }


        void Instance_OnFight()
        {
            var tile = ShellTile.ActiveTiles.First();
            var tileData = new StandardTileData()
            {
                Count = AI.Instance.CurrentNumber.LevelNumber,
                BackContent = AI.Instance.CurrentMoney.ToString() + "$ \n "+AI.Instance.CurrentNumber.Number.ToString()
            };

            tile.Update(tileData);
        }


    }
}
