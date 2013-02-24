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
    public partial class TileSimpleVoidElement : AbstractTile
    {
        public TileSimpleVoidElement()
        {
            InitializeComponent();
            LayoutRoot.Opacity = 0;
            LayoutRoot.DataContext = this;
            this.Loaded += FormMultiVoidElement_Loaded;
            ResourceDictionary SBResources = new ResourceDictionary()
            {
                Source = new Uri("/FstnUserControl;component/Resources/TileStoryBoard.xaml", UriKind.Relative)
            };
            ShowSB = SBResources["ShowSB"] as Storyboard;
            CloseSB = SBResources["CloseSB"] as Storyboard;

            Storyboard.SetTarget(ShowSB, LayoutRoot);
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
            LayoutRoot.Children.Add(Element);
            LayoutRoot.Background = Background;
            this.Loaded -= FormMultiVoidElement_Loaded;
        }

    }
}

