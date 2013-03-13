using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BrokeThePig.Source.AI;
using BrokeThePig.UC.Weapons;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BrokeThePig.UC
{
    public delegate void ExplodedHandleEvent();
    public partial class Pig : UserControl
    {
        public bool ended = false;
        public event ExplodedHandleEvent Exploded;
        SoundController sc;
        public Pig()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += Pig_Loaded;
            this.Unloaded+=Pig_Unloaded;
        }


        void Pig_Loaded(object sender, RoutedEventArgs e)
        {
            ExplodeEffect.Show();
            AI.Instance.LevelEnded += OnLevelEnded;
            this.Visibility = Visibility.Visible;
            
            String GiftUri = (String)AI.Instance.CurrentLevel.GiftImage;
            Gift.Source = new BitmapImage(new Uri(GiftUri, UriKind.RelativeOrAbsolute));
        }

        void OnLevelEnded()
        {
            if (!ended)
            {
                ended = true;
                ExplodeEffect.Show();
                Body_before.Visibility = Visibility.Collapsed;
                ExplodeEffect.RunExplosion();
                Body_broke.Clip = null;
                Body_broke.Visibility = Visibility.Visible;
                Body_before.Visibility = Visibility.Collapsed;
                explode.Completed += OnExplodeComplete;
                explode.Begin();
            }
        }

        void OnExplodeComplete(object sender, EventArgs e)
        {
            Body_broke.Visibility = Visibility.Collapsed;
            sc = new SoundController();
            sc.PlaySound("Sounds/win.wav");
            if (Exploded != null)
            {
                Exploded();
            }
        }

        void Pig_Unloaded(object sender, RoutedEventArgs e)
        {
            RootLayout.Children.Clear();
            this.Loaded -= Pig_Loaded;
            this.Unloaded -= Pig_Unloaded;
            Exploded = null;
            sc = null;
        }
    }
}
