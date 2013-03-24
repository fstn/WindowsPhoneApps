using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using FstnCommon;
namespace FstnUserControl
{
    public partial class TimeCounter : UserControl
    {

        public static readonly DependencyProperty EventDateProperty;
        public static readonly DependencyProperty UpdateTimeProperty;
        public event ChangedEventHandler Changed;
        private DispatcherTimer dt;
        public TextBlock Text { get; set; }
        public TimeCounter()
        {
            InitializeComponent();
            Text = new TextBlock();
            var uri = new Uri("Fonts/digital-7.ttf", UriKind.Relative);
            var streamInfo = Application.GetResourceStream(uri);
            Text.FontSource = new FontSource(streamInfo.Stream);
            Text.FontFamily = new FontFamily("Digital-7");
            Text.FontSize = 80;
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleY = myScaleTransform.ScaleY * 1.5;
            myScaleTransform.CenterY=0;
            Text.RenderTransform = myScaleTransform;
            Text.TextAlignment = TextAlignment.Center;
            LayoutRoot.Children.Add(Text);
            Text.Text = "";
            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, UpdateTime);
            dt.Tick += new EventHandler(dt_Tick);
        }

        static TimeCounter()
        {
            UpdateTimeProperty = DependencyProperty.Register("UpdateTime", typeof(int), typeof(TimeCounter), new PropertyMetadata(500));
            EventDateProperty = DependencyProperty.Register("EventDate", typeof(DateTime), typeof(TimeCounter), new PropertyMetadata(new DateTime()));
        }
        public int UpdateTime
        {
            get { return (int)GetValue(UpdateTimeProperty); }
            set { SetValue(UpdateTimeProperty, value); }
        }


        public DateTime EventDate
        {
            get { return (DateTime)GetValue(EventDateProperty); }
            set { SetValue(EventDateProperty, value); }
        }

        public void StartCount(){            
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            DateTime now=DateTime.Now;
            TimeSpan diff = (TimeSpan)(EventDate - now);
            if (diff > new TimeSpan(0))
            {
                String hours = ((int)diff.TotalHours).ToString();
                Text.Text = (hours.Length < 2 ? "0" + hours : hours) + ":" 
                    + (diff.Minutes.ToString().Length < 2 ? "0" + diff.Minutes.ToString() : diff.Minutes.ToString())
                    + ":" + (diff.Seconds.ToString().Length < 2 ? "0" + diff.Seconds.ToString() : diff.Seconds.ToString());
            }
            else
            {
                if (Changed != null)
                    Changed(this, null);
                dt.Stop();
            }
        }

    }
}
