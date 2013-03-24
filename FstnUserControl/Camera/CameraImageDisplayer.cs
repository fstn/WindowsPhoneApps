using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FstnAnimation.Dynamic;
using Microsoft.Devices;
using Microsoft.Phone.Controls;

namespace FstnUserControl.Camera
{
    public class CameraImageDisplayer : Canvas
    {
        #region fields
        private Shape imageShape;
        public Shape ImageShape
        {
            get
            {
                return imageShape;
            }
            set
            {
                imageShape = value;
            }
        }
        public OrientedCameraImage OrientedImage { get; set; }
        public ImageBrush ImageBrushImage { get; set; }
        public Canvas CanvasImage { get; set; }
        public Point Position { get; set; }
        private CompositeTransform ThisTransform;
        private PageOrientation displayOrientation;
        public PageOrientation DisplayOrientation
        {
            set
            {
                displayOrientation = value;
            }
        }
        private CameraType type;
        public CameraType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        private double initWidth;
        public double InitWidth
        {
            get
            {
                return initWidth;
            }
            set
            {
                initWidth = value;
                Width = value;
            }
        }
        private double initHeight;
        public double InitHeight
        {
            get
            {
                return initHeight;
            }
            set
            {
                initHeight = value;
                Height = value;
            }
        }

        //use for move to 
        private Point topLeft;
        public Point TopLeft
        {
            get
            {
                return topLeft;
            }
            set
            {
                topLeft = value;
            }
        }
        private Point bottomRight;
        public Point BottomRight
        {
            get
            {
                Point value = new Point();
                switch (displayOrientation)
                {
                    case PageOrientation.Landscape:
                    case PageOrientation.LandscapeLeft:
                        value = bottomRight;
                        break;
                    case PageOrientation.LandscapeRight:
                        value = bottomRight;
                        break;
                    case PageOrientation.Portrait:
                    case PageOrientation.PortraitUp:
                        value.Y = bottomRight.X;
                        value.X = 0.75 * value.Y;
                        break;
                    case PageOrientation.PortraitDown:
                        value.Y = bottomRight.X;
                        value.X = 0.75 * value.Y;
                        break;
                }
                return value;
            }
            set
            {
                bottomRight = value;
            }
        }
        private MoveEffect me;

        private Binding HeightBinding;
        private Binding WidthBinding;

        public Point PercentPosition
        {
            get
            {
                Point ret = new Point(0,0);
                if (Position != null )
                {
                    ret.X = Position.X / BottomRight.X;
                    ret.Y = Position.Y/ BottomRight.Y ;
                }
                return ret;
            }
        }
        #endregion
        #region ctor
        public CameraImageDisplayer(Shape NewImageShape)
        {
            ImageShape = NewImageShape;
            imageShape.Hold += imageShape_Hold;
            ImageBrushImage = new ImageBrush();
            ThisTransform = new CompositeTransform();
            this.RenderTransformOrigin = new Point(0, 0);
            this.RenderTransform = ThisTransform;
            TopLeft = new Point(int.MinValue, int.MinValue);
            BottomRight = new Point(int.MaxValue, int.MaxValue);
            CanvasImage = new Canvas();
            CanvasImage.Children.Add(ImageShape);
            this.Children.Add(CanvasImage);
            me = new MoveEffect();

            HeightBinding = new Binding();
            HeightBinding.Source = this;
            HeightBinding.Path = new PropertyPath("Height");

            WidthBinding = new Binding();
            WidthBinding.Source = this;
            WidthBinding.Path = new PropertyPath("Width");

            ImageShape.SetBinding(TextBlock.HeightProperty, HeightBinding);
            ImageShape.SetBinding(TextBlock.WidthProperty, WidthBinding);
            this.SetBinding(Canvas.HeightProperty, HeightBinding);
            this.SetBinding(Canvas.WidthProperty, WidthBinding);
            Position = new Point(0,0);
        }

        void imageShape_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ThisTransform.Rotation += 90;
        }
        #endregion

        #region method

        public void Load(OrientedCameraImage newOrientedImage)
        {
            if (newOrientedImage != null)
            {
                OrientedImage = newOrientedImage;

                ImageBrushImage = new ImageBrush();
                ImageBrushImage.ImageSource = OrientedImage.Image;
                ImageShape.Fill = ImageBrushImage;

                double ratio = Math.Max(initWidth, initHeight) / (Math.Max(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight));

                if (OrientedImage.Orientation == PageOrientation.PortraitDown)
                {
                    Width = ratio * Math.Min(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                    Height = ratio * Math.Max(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                }
                if (OrientedImage.Orientation == PageOrientation.PortraitUp)
                {
                    Width = ratio * Math.Min(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                    Height = ratio * Math.Max(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                }
                if (OrientedImage.Orientation == PageOrientation.LandscapeLeft)
                {
                    Width = ratio * Math.Max(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                    Height = ratio * Math.Min(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                }
                if (OrientedImage.Orientation == PageOrientation.LandscapeRight)
                {
                    Width = ratio * Math.Max(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                    Height = ratio * Math.Min(OrientedImage.Image.PixelWidth, OrientedImage.Image.PixelHeight);
                }
              
            }
            else
                Debugger.Log(0, "", "OrientedImage is null in" + this.ToString());
        }

        public void MoveTo(Point NewPosition)
        {
            if (NewPosition.X < TopLeft.X + this.ActualWidth)
                NewPosition.X = TopLeft.X;
            if (NewPosition.X > BottomRight.X - this.ActualWidth)
                NewPosition.X = BottomRight.X - this.ActualWidth;

            if (NewPosition.Y < TopLeft.Y + this.ActualHeight)
                NewPosition.Y = TopLeft.Y;
            if (NewPosition.Y > BottomRight.Y - this.ActualHeight)
                NewPosition.Y = BottomRight.Y - this.ActualHeight;
           
            me.From = Position;
            me.To = NewPosition;
            me.Duration = 500;
            me.Speed = 1;
            me.Transform = ThisTransform;
            me.Mode = EasingMode.EaseOut;
            me.EasingFunction = new CircleEase();
            me.Start(this);
            me.Completed += (s, e) =>
            {
                Position = NewPosition;
            };
        }
        #endregion
    }
}
