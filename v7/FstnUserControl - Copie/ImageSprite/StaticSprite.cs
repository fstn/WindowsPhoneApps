using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FstnUserControl.ImageSprite
{
    public class StaticSprite : Canvas
    {

        #region DependencyProperty

        public int ContentWidth
        {
            get { return (int)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register("ContentWidth", typeof(int), typeof(StaticSprite), new PropertyMetadata(0));

        public int ContentHeight
        {
            get { return (int)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register("ContentHeight", typeof(int), typeof(StaticSprite), new PropertyMetadata(0));

        public int Length
        {
            get { return (int)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Lenght.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(int), typeof(StaticSprite), new PropertyMetadata(0));

        #endregion

        #region fields
            private Image DisplayImage;
            private RectangleGeometry DisplayClip;
            private int CurrentNumber;
        #endregion

        public StaticSprite()
        {
            this.Visibility = Visibility.Collapsed;
            DisplayClip = new RectangleGeometry();
            this.Loaded += StaticSprite_Loaded;
        }

        void StaticSprite_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayImage = (Image)this.Children.FirstOrDefault();
            DisplayImage.Clip = DisplayClip;
            MoveToFirst();
            this.Visibility = Visibility.Visible;
        }

        public void MoveToNext()
        {
            if (CurrentNumber < Length - 1)
            {
                CurrentNumber++;
                UpdateClip();
            }
        }

        private void UpdateClip()
        {
            DisplayClip.Rect = new Rect(CurrentNumber * ContentWidth, 0, ContentWidth, ContentHeight);
            Canvas.SetLeft(DisplayImage, -CurrentNumber * ContentWidth);
        }

        public void MoveTo(int Number)
        {
            if (Number > 0 && Number < Length)
            {
                CurrentNumber = Number;
                UpdateClip();
            }
        }
        public void MoveToFirst()
        {
            CurrentNumber = 0;
            UpdateClip();
        }
    }
}
