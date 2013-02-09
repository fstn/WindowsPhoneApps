using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimeCounter
{
    public class TimeCounter : TextBox
    {
        public int UpdateTime
        {
            get { return (int)GetValue(UpdateTimeProperty); }
            set { SetValue(UpdateTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateTimeProperty =
            DependencyProperty.Register("UpdateTime", typeof(int), typeof(TimeCounter), new PropertyMetadata(0));

        

        public TimeCounter()
        {
        }
    }
}
