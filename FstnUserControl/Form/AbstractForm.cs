using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using FstnCommon;

namespace FstnUserControl.Form
{
    public abstract class AbstractForm : MyUserControl
    {

        protected Storyboard ShowSB;
        protected Storyboard CloseSB;
        #region depency property
        public String View
        {
            get { return (String)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }
        public FrameworkElement Element
        {
            get { return (FrameworkElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        public String LabelText
        {
            get { return (String)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(String), typeof(AbstractForm), new PropertyMetadata(""));

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(AbstractForm), new PropertyMetadata(new TextBlock()));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(String), typeof(AbstractForm), new PropertyMetadata("Toto"));

        #endregion

    }
}
