using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace WBEX.SWE.Performance
{
    public class MainPage : UserControl
    {
#if WindowsCE
	//Code required to reference XAML objects
        internal System.Windows.Controls.Grid LayoutRoot;
        internal System.Windows.Controls.Image ImageViewport;
        internal System.Windows.Controls.TextBox TxtBoxShapeCount;
        internal System.Windows.Controls.TextBlock TxtBlockPerf;
        internal Grid ViewPortContainer;
        private bool _contentLoaded;

        [System.Diagnostics.DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!_contentLoaded)
            {
                _contentLoaded = true;
                System.Windows.Application.LoadComponent(this, new Uri("/WBEX.SWE.Performance;component/MainPage.xaml", UriKind.Relative));
                this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
                this.ImageViewport = ((System.Windows.Controls.Image)(this.FindName("ImageViewport")));
                this.TxtBoxShapeCount = ((System.Windows.Controls.TextBox)(this.FindName("TxtBoxShapeCount")));
                this.TxtBlockPerf = ((System.Windows.Controls.TextBlock)(this.FindName("TxtBlockPerf")));
                this.ViewPortContainer = (Grid)FindName("ViewPortContainer");
            }
        }
#endif
        private InTheHand.Windows.Threading.DispatcherTimer timer = new InTheHand.Windows.Threading.DispatcherTimer();
        private WriteableBitmap writeableBmp;
        private int shapeCount;
        private static Random rand = new Random();
        private int frameCounter = 0;

        public MainPage()
        {
            // Required to initialize variables
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.TxtBoxShapeCount.TextChanged += new TextChangedEventHandler(TxtBoxShapeCount_TextChanged);
        }

        void TxtBoxShapeCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            int v = 1;
            try
            {
                v = int.Parse(TxtBoxShapeCount.Text);

                this.shapeCount = v;
                TxtBoxShapeCount.Background = new SolidColorBrush(Colors.White);
                frameCounter = 0;
                Draw();
            }
            catch
            {
                TxtBoxShapeCount.Background = new SolidColorBrush(Colors.Red);
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            timer.Interval = new TimeSpan(16);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private int f = 0;
        private TimeSpan all;
        void timer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();
            Draw();
            st.Stop();
            var span = st.Elapsed;
            all += span;
            f++;
            TxtBlockPerf.Text = String.Format("{0:f2} ms / frame", all.TotalMilliseconds / f);

            if (f > 10)
            {
                f = 0;
                all = TimeSpan.FromTicks(0);
            }
        }


        private void Init()
        {
            // Show fps counter
            //Application.Current.Host.Settings.EnableFrameRateCounter = true;

            // Init WriteableBitmap
            writeableBmp = new WriteableBitmap(ViewPortContainer, new TranslateTransform());//(bs);//((int)ViewPortContainer.Width, (int)ViewPortContainer.Height);
            ImageViewport.Source = writeableBmp;
        }

        private void Draw()
        {
            DrawShapes();
        }

        /// <summary>
        /// Draws random shapes.
        /// </summary>
        private void DrawShapes()
        {
            if (writeableBmp == null)
            {
                Init();
                timer.Interval = new TimeSpan(16);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            using (var bitmapContext = writeableBmp.GetBitmapContext())
            {
                // Init some size vars
                int w = this.writeableBmp.PixelWidth - 2;
                int h = this.writeableBmp.PixelHeight - 2;
                int wh = w >> 1;
                int hh = h >> 1;

                // Clear 
                writeableBmp.Clear();

                // Draw Shapes and use refs for faster access which speeds up a lot.
                int wbmp = writeableBmp.PixelWidth;
                int hbmp = writeableBmp.PixelHeight;
                int[] pixels = writeableBmp.Pixels;
                for (int i = 0; i < shapeCount / 6; i++)
                {
                    // Standard shapes
                    WriteableBitmapExtensions.DrawLine(bitmapContext, wbmp, hbmp, rand.Next(w), rand.Next(h), rand.Next(w),
                                                       rand.Next(h), GetRandomColor());
                    writeableBmp.DrawTriangle(rand.Next(w), rand.Next(h), rand.Next(w), rand.Next(h), rand.Next(w),
                                              rand.Next(h), GetRandomColor());
                    writeableBmp.DrawQuad(rand.Next(w), rand.Next(h), rand.Next(w), rand.Next(h), rand.Next(w),
                                          rand.Next(h), rand.Next(w), rand.Next(h), GetRandomColor());
                    writeableBmp.DrawRectangle(rand.Next(wh), rand.Next(hh), rand.Next(wh, w), rand.Next(hh, h),
                                               GetRandomColor());
                    writeableBmp.DrawEllipse(rand.Next(wh), rand.Next(hh), rand.Next(wh, w), rand.Next(hh, h),
                                             GetRandomColor());

                    // Random polyline
                    int[] p = new int[rand.Next(5, 10) * 2];
                    for (int j = 0; j < p.Length; j += 2)
                    {
                        p[j] = rand.Next(w);
                        p[j + 1] = rand.Next(h);
                    }
                    writeableBmp.DrawPolyline(p, GetRandomColor());
                }

                // Invalidate
                writeableBmp.Invalidate();
            }
        }

        /// <summary>
        /// Random color fully opaque
        /// </summary>
        /// <returns></returns>
        private static int GetRandomColor()
        {
            return (int)(0xFF000000 | (uint)rand.Next(0xFFFFFF));
        }
    }
}
