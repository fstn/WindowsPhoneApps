using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using FstnCommon;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl.Tile
{
    public abstract class AbstractTile : MyUserControl
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

        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(String), typeof(AbstractTile), new PropertyMetadata(""));

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(AbstractTile), new PropertyMetadata(new TextBlock()));

        #endregion
       
    }
}
