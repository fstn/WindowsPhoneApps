﻿#pragma checksum "C:\DevTools\Git\windowsPhone\Weekend\FstnUserControl\Tile\AbstractTile.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "45394978D5E93061F6F781A5FAA964DF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FstnCommon;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace FstnUserControl.Tile {
    
    
    public partial class AbstractTile : FstnCommon.MyUserControl {
        
        internal System.Windows.Media.Animation.Storyboard ShowSB;
        
        internal System.Windows.Media.Animation.Storyboard CloseSB;
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/FstnUserControl;component/Tile/AbstractTile.xaml", System.UriKind.Relative));
            this.ShowSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("ShowSB")));
            this.CloseSB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("CloseSB")));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
        }
    }
}

