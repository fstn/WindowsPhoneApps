using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FstnAnimation.Dynamic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl
{
    public delegate void CompletedCoinFlipEventHandler(object sender, EventArgs e);
    public partial class CoinFlip : UserControl
    {
        public event CompletedCoinFlipEventHandler Completed;

        private Queue<FrameworkElement> q;
        private RotateEffect effect;
        public static readonly DependencyProperty Image1SourceProperty;
        public static readonly DependencyProperty Text1Property;
        public static readonly DependencyProperty Image2SourceProperty;
        public static readonly DependencyProperty Text2Property;
        public static readonly DependencyProperty TossProperty;
        private Boolean ended = true;
        
        public CoinFlip()
        {
            InitializeComponent();
        }
        static CoinFlip()
        {
            TossProperty = DependencyProperty.Register("Toss", typeof(Boolean), typeof(CoinFlip), new PropertyMetadata(true));           
            Image1SourceProperty = DependencyProperty.Register("Image1Source", typeof(String), typeof(CoinFlip), new PropertyMetadata(""));
            Text1Property = DependencyProperty.Register("Text1", typeof(String), typeof(CoinFlip), new PropertyMetadata(""));
            Image2SourceProperty = DependencyProperty.Register("Image1Source", typeof(String), typeof(CoinFlip), new PropertyMetadata(""));
            Text2Property = DependencyProperty.Register("Text2", typeof(String), typeof(CoinFlip), new PropertyMetadata(""));
          
        }
        public Boolean Toss
        {
            get { return (Boolean)GetValue(TossProperty); }
            set { SetValue(TossProperty, value); }
        }
        public String Image1Source
        {
            get { return (String)GetValue(Image1SourceProperty); }
            set
            {
                SetValue(Image1SourceProperty, value);
                Uri uri = new Uri(Image1Source, UriKind.Relative);
                if (uri != null)
                {
                    finalImage.Source = new BitmapImage(uri);
                }
            }
        }
        public String Image2Source
        {
            get { return (String)GetValue(Image2SourceProperty); }
            set
            {
                SetValue(Image2SourceProperty, value);
                Uri uri = new Uri(Image2Source, UriKind.Relative);
                if (uri != null)
                {
                    nonFinalImage.Source = new BitmapImage(uri);
                }
            }
        }

        void Image_Loaded(object sender, RoutedEventArgs e)
        {
            /* if (Loaded != null)
                 Loaded(this, null);*/
        }
        public String Text1
        {
            get { return (String)GetValue(Text1Property); }
            set
            {
                SetValue(Text1Property, value);
               // finalTextBlock.Text = FinalText;
            }
        }
        public String Text2
        {
            get { return (String)GetValue(Text2Property); }
            set
            {
                SetValue(Text2Property, value);
              //  nonFinalTextBlock.Text = NonFinalText;
            }
        }
        private void finalResponseImg_Loaded(object sender, EventArgs e)
        {
        }
        public void effect_Completed(object sender, EventArgs e)
        {
            FrameworkElement first = q.Dequeue();
            FrameworkElement second = q.Dequeue();
            effect.Speed = effect.Speed-0.5;
            first.Visibility = Visibility.Visible;
            second.Visibility = Visibility.Collapsed;
            if (effect.Speed > 0.25)
            {
                q.Enqueue(second);
                q.Enqueue(first);
            }
            else
            {
                effect.Speed = 0.25;
                effect.To = 0;
                effect.Completed -= effect_Completed;
                ended = true;

                if (this.Completed != null)
                {
                    Completed(this, null);
                }
            }
           /* if(effect.Speed <1){
                cote.Visibility = Visibility.Visible;
                collapseEffect.Start(cote);
            }*/
            effect.Start(first);
        }
        public void Start()
        {
            if (ended)
            {
                ended = false;
                nonFinalCanvas.Visibility = Visibility.Visible;
                finalCanvas.Visibility = Visibility.Collapsed;
                EasingFunctionBase easing = new CubicEase();
                effect = new RotateEffect(-90, 90, 6, EasingMode.EaseOut, easing);
                effect.Completed += effect_Completed;
                q = new Queue<FrameworkElement>();
                if (Toss)
                {
                    effect.Start(finalCanvas);
                    q.Enqueue(nonFinalCanvas);
                    q.Enqueue(finalCanvas);
                }
                else
                {
                    effect.Start(nonFinalCanvas);
                    q.Enqueue(finalCanvas);
                    q.Enqueue(nonFinalCanvas);
                }
            }
        }
    }
}
