using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using FstnCommon;
using FstnUserControl.Tile;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FstnUserControl.Tile
{
    public partial class TileMultiVoidElement : AbstractTile
    {
      #region depency property
     
        public String LabelText
        {
            get { return (String)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }
              
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(String), typeof(TileMultiVoidElement), new PropertyMetadata("Toto"));

        #endregion

        public TileMultiVoidElement()
        {
            InitializeComponent();
            LayoutRoot.Opacity = 0;
            LayoutRoot.DataContext = this;
            this.Loaded += FormMultiVoidElement_Loaded;
            ShowSB = this.Resources["ShowSB"] as Storyboard;
            CloseSB = this.Resources["CloseSB"] as Storyboard;

            Storyboard.SetTarget(ShowSB,LayoutRoot);
            Storyboard.SetTarget(CloseSB, LayoutRoot);

        }

        public override void Show()
        {
            ShowSB.Begin();
        }
        public override void Close()
        {
            CloseSB.Begin();
        }
        void FormMultiVoidElement_Loaded(object sender, RoutedEventArgs e)
        {
            Session.Instance.addUserControl(View, this);
            if (SecondLine.Children.Count() == 0)
            {
                Label.Text = LabelText;
                SecondLine.Children.Add(Element);
            }
            this.Loaded -= FormMultiVoidElement_Loaded;
            LayoutRoot.Background = Background;    
        }
    }
}
