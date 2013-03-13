using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FstnAnimation.Dynamic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl
{
    public partial class ExplodeEffect : UserControl
    {


        public String PixelColor
        {
            get { return (String)GetValue(PixelColorProperty); }
            set { SetValue(PixelColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PixelColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PixelColorProperty =
            DependencyProperty.Register("PixelColor", typeof(String), typeof(ExplodeEffect), new PropertyMetadata(""));

        private List<UIElement> Pixels;
        private Random RandomNumber;

        public ExplodeEffect()
        {
            InitializeComponent();
            Pixels = new List<UIElement>();
            this.Loaded += ExplodeEffect_Loaded;
            this.Unloaded += ExplodeEffect_Unloaded;
        }

        void ExplodeEffect_Unloaded(object sender, RoutedEventArgs e)
        {
            ExplosionEffect.Children.Clear();
        }

        void ExplodeEffect_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void Show()
        {

            Color pixClr = new Color();
            pixClr.A = 255;
            pixClr.R = byte.Parse(PixelColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            pixClr.G = byte.Parse(PixelColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            pixClr.B = byte.Parse(PixelColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            RandomNumber = new Random();
            for (int i = 0; i < 100; i++)
            {


                int xval = RandomNumber.Next((int)Width);
                int yval = RandomNumber.Next((int)Height);
                int xRval = xval+RandomNumber.Next(-20,20);
                int yRval = yval + RandomNumber.Next(-20, 20);
                int xR2val = xRval + RandomNumber.Next(-20, 20);
                int yR2val = yRval + RandomNumber.Next(-20, 20);
                Point startingPt = new Point(xval, yval);
                Point randomPt = new Point(xRval, yRval);
                Point randomPt2 = new Point(xR2val, yR2val);
                Path pixel = new Path();
                PathFigure pixelFigure = new PathFigure();
                GeometryGroup geometry = new GeometryGroup();
                LineSegment line1 = new LineSegment();
                pixelFigure.StartPoint = startingPt;
                line1.Point = randomPt;

                LineSegment line2 = new LineSegment();
                line2.Point = randomPt2;

                LineSegment line3 = new LineSegment();
                line3.Point = startingPt;
                pixelFigure.Segments.Add(line1);
                pixelFigure.Segments.Add(line2);
                pixelFigure.Segments.Add(line3);
                
                pixel.Stroke = new SolidColorBrush(pixClr);
                pixel.Fill = new SolidColorBrush(pixClr);
               // pixel.Stretch = Stretch.Fill;
                PathGeometry pathGeo = new PathGeometry();
                pathGeo.Figures.Add(pixelFigure);
                pixel.Data = pathGeo;
                ExplosionEffect.Children.Add(pixel);
            }
        }
        public void RunExplosion()
        {

            foreach (Path pixel in ExplosionEffect.Children)
            {
                MoveEffect me = new MoveEffect();
                double toLeft = RandomNumber.Next(-500, 500);
                double toTop =RandomNumber.Next(-500, 500);
               
                me.From = new Point(0, 0);
                if (toLeft < 0 && toLeft > -300)
                {
                    toLeft -= 500;
                }
                if (toLeft > 0 && toLeft < 300)
                {
                    toLeft += 500;
                }
                if (toTop < 0 && toTop > -300)
                {
                    toTop -= 500;
                }
                if (toTop > 0 && toTop < 300)
                {
                    toTop += 500;
                }
                me.To = new Point(toLeft, toTop);

                me.EasingFunction = new CircleEase();
                me.Duration = 2000;
                me.Mode = EasingMode.EaseIn;
                me.Start(pixel);

            }
        }
    }
}
