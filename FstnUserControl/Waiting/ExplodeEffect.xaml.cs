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
        }

        void ExplodeEffect_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void Show()
        {
            /*
        <Path x:Name="Fragment3" Data="M21.6068,83.1675 L30.6524,75.1268 L47.7388,80.1521 L45.7287,90.203 L53.7692,99.2488 L47.5553,108.653 L29.2748,84.531 L5.50998,82.6952 L35.6778,103.269 L10.6873,78.5386 z" Fill="#FFF4F4F5" HorizontalAlignment="Left" Height="34.357" Margin="118.234,140.75,0,0" Stretch="Fill" Stroke="Black" UseLayoutRounding="False" VerticalAlignment="Top" Width="49.016"/>
        <Path x:Name="Fragment4" Data="M21.6068,83.1675 L30.6524,75.1268 L29.6476,88.1927 L45.7287,90.203 L36.6831,101.259 L47.5553,108.653 L30.6529,96.2333 L5.50998,82.6952 L35.6778,103.269 L10.6873,78.5386 z" Fill="#FFF4F4F5" HorizontalAlignment="Left" Height="34.357" Margin="118.234,140.75,0,0" Stretch="Fill" Stroke="Black" UseLayoutRounding="False" VerticalAlignment="Top" Width="42.833"/>
        <Path x:Name="Fragment1" Data="M21.6068,83.1675 L30.6524,75.1268 L58.7951,84.1725 L47.7393,97.2384 L36.6831,101.259 L47.5553,108.653 L60.8053,85.1775 L5.50998,82.6952 L35.6778,103.269 L10.6873,78.5386 z" Fill="#FFF4F4F5" HorizontalAlignment="Left" Height="34.357" Margin="118.234,140.75,0,0" Stretch="Fill" Stroke="Black" UseLayoutRounding="False" VerticalAlignment="Top" Width="56.016"/>
        <Path x:Name="Fragment2" Data="M21.6068,83.1675 L37.6882,93.2181 L47.7388,80.1521 L45.7287,90.203 L43.7185,105.279 L47.5553,108.653 L15.5767,104.274 L5.50998,82.6952 L35.6778,103.269 L10.6873,78.5386 z" Fill="#FFF4F4F5" HorizontalAlignment="Left" Height="30.962" Margin="118.234,144.145,0,0" Stretch="Fill" Stroke="Black" UseLayoutRounding="False" VerticalAlignment="Top" Width="43.016"/>
            */

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
