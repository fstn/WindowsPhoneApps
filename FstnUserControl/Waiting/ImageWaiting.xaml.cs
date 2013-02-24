using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using FstnAnimation.Dynamic;
using FstnDesign;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


/**
 * ImageWaiting
 * Animation to help user to pe patient ...
 * Anime SecretAnimationControl to fill the screen
 * Stephen ZAMBAUX
 **/


namespace FstnUserControl
{
    public delegate void BackgroundIsMaskAnimationEventHandler();
    public partial class ImageWaiting : UserControl
    {
        #region members
        private double secretElementWidth;
        private double secretElementHeight;
        private Random random;

        private List<Point> pointsAreadyDone;
        private Queue<Point> tmpQueueForBinaryTree;
        private List<SecreteAnimationControl> listOfElements;
        private Queue<SecreteAnimationControl> animStartOfElements;
        private Queue<SecreteAnimationControl> animStopOfElements;
        private DispatcherTimer animTimer;
        private Boolean stopped = false;
        private Boolean started = true;
        private int StartAnimationDuration { get; set; }
        private int StopAnimationDuration { get; set; }
        private int StartAnimationTimer { get; set; }
        private int StopAnimationTimer { get; set; }
        private int StartColorTimer { get; set; }

        //Publish when background is masked 
        public event StopSecreteAnimationEventHandler MaskedEvent;

        //default value
        private int nbEltPerLine = 8;

        public int NbEltPerLine
        {
            get { return nbEltPerLine; }
            set { nbEltPerLine = value; }
        }

        private int nbEltPerColumn = 12;
        public int NbEltPerColumn
        {
            get { return nbEltPerColumn; }
            set { nbEltPerColumn = value; }
        }

        public bool Masked { get; set; }
        #endregion

        public ImageWaiting()
        {
            InitializeComponent();

            StartAnimationDuration = 500;
            StopAnimationDuration = 500;
            StartAnimationTimer = 10;
            StopAnimationTimer = 10;
            StartColorTimer = 50;


            listOfElements = new List<SecreteAnimationControl>();
            animStartOfElements = new Queue<SecreteAnimationControl>();
            animStopOfElements = new Queue<SecreteAnimationControl>();
            pointsAreadyDone = new List<Point>();
            random = new Random(DateTime.Now.Millisecond);
        }


        private void loadElt(Point p)
        {
            pointsAreadyDone.Add(p);

            SecreteAnimationControl elt = new SecreteAnimationControl();
            elt.Width = secretElementWidth;
            elt.Height = secretElementHeight;

            //RootCanvas
            RootCanvas.Children.Add(elt);
            RootCanvas.Visibility = Visibility.Visible;
            int YIndice = (int)p.Y;
            int XIndice = (int)p.X;
            Canvas.SetLeft(elt, XIndice * (secretElementWidth));
            Canvas.SetTop(elt, YIndice * (secretElementHeight));

            elt.ForegroundColor = ColorManager.Instance.RandomAccentBrush;
            MoveEffect me = new MoveEffect(new Point(0, -1.1 * Width), new Point(0, 0), 1, StartAnimationDuration, EasingMode.EaseIn, new CircleEase());
            RotateEffect re = new RotateEffect(-90, 0, 1,StartAnimationDuration, EasingMode.EaseOut, new CircleEase());
            elt.addStartEffect(me);
            elt.addStartEffect(re);
            MoveEffect meStop = new MoveEffect( new Point(0, 0),new Point(0, 1.1 * Height), 1, StopAnimationDuration, EasingMode.EaseIn, new CircleEase());
            RotateEffect reStop = new RotateEffect(0, -90, 1,StopAnimationDuration, EasingMode.EaseOut, new CircleEase());
            elt.addStopEffect(meStop);
            elt.addStopEffect(reStop);
            elt.load();
            listOfElements.Add(elt);
            animStartOfElements.Enqueue(elt);
            animStopOfElements.Enqueue(elt);

        }

        public Boolean isValid(Point p)
        {
            Boolean retour = false;
            if ((int)p.X < nbEltPerLine && (int)p.Y < nbEltPerColumn && !pointsAreadyDone.Contains(p) && !tmpQueueForBinaryTree.Contains(p))
            {
                retour = true;
            }
            return retour;
        }

        public void start()
        {
            Masked = false;
            secretElementWidth = Width / nbEltPerLine;
            secretElementHeight = Height / nbEltPerColumn;

            int nbTotal = nbEltPerColumn * nbEltPerLine;

            // fill queue with binary tree
            Point p = new Point(0, 0);
            tmpQueueForBinaryTree = new Queue<Point>();
            tmpQueueForBinaryTree.Enqueue(new Point(0, 0));
            while (listOfElements.Count < nbTotal && tmpQueueForBinaryTree.Count > 0)
            {
                Point point = tmpQueueForBinaryTree.Dequeue();
                loadElt(point);
                Point pX = new Point(point.X + 1, point.Y);
                if (isValid(pX))
                {
                    tmpQueueForBinaryTree.Enqueue(pX);
                }
                Point pY = new Point(point.X, point.Y + 1);
                if (isValid(pY))
                {
                    tmpQueueForBinaryTree.Enqueue(pY);
                }
            }

            animStopOfElements = new Queue<SecreteAnimationControl>(animStopOfElements.Reverse());
            animStartOfElements = new Queue<SecreteAnimationControl>(animStartOfElements.Reverse());
            pointsAreadyDone.Clear();


            animTimer = new DispatcherTimer();
            animTimer.Interval = new TimeSpan(0, 0, 0, 0, StartAnimationTimer);
            animTimer.Tick += start_Tick;
            animTimer.Start();
        }

        void start_Tick(object sender, EventArgs e)
        {
            StartAnimation();
        }
        void stop_Tick(object sender, EventArgs e)
        {
            StopAnimation();
        }
        void color_Tick(object sender, EventArgs e)
        {
            ColorAnimation();
        }

        public void StartAnimation()
        {
            if (animStartOfElements.Count > 0)
            {
                SecreteAnimationControl elt = animStartOfElements.Dequeue();
                elt.start();
            }
            else
            {
                if (MaskedEvent != null)
                {
                    MaskedEvent();
                }
                Masked = true;
                animTimer.Stop();
                started = false;
                if (stopped)
                {
                    LaunchStopAnimation();
                }
                else
                {
                    LaunchColorAnimation();
                }
            }
        }

        public void LaunchColorAnimation()
        {
            animTimer.Stop();
            animTimer = new DispatcherTimer();
            animTimer.Interval = new TimeSpan(0, 0, 0, 0, StartColorTimer);
            animTimer.Tick += color_Tick;
            animTimer.Start();
        }

        public void ColorAnimation()
        {
            SecreteAnimationControl elt = listOfElements.ElementAt(random.Next(0, listOfElements.Count));
            elt.ChangeColor(ColorManager.Instance.RandomAccentBrush);
        }

        public void LaunchStopAnimation()
        {
            Masked = false;
            stopped = true;
            if (started == false)
            {
                animTimer.Stop();
                animTimer = new DispatcherTimer();
                animTimer.Interval = new TimeSpan(0, 0, 0, 0, StopAnimationTimer);
                animTimer.Tick += stop_Tick;
                animTimer.Start();
            }
            started = false;
        }

        public void StopAnimation()
        {
            if (animStopOfElements.Count > 0)
            {
                SecreteAnimationControl elt = animStopOfElements.Dequeue();
                elt.stop();
            }
            else
            {
                animTimer.Stop();
            }
        }

        internal void Clean()
        {
            animTimer.Stop();
            animTimer = null;
            listOfElements = null;
            animStartOfElements = null;
            animStopOfElements = null;
            MaskedEvent = null;
        }

    }
}
