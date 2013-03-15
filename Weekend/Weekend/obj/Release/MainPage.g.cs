﻿#pragma checksum "C:\DevTools\Git\windowsPhone\Weekend\Weekend\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FC01E44FD09940CCD7DFB9BBB86B6197"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FstnUserControl;
using FstnUserControl.Tile;
using Microsoft.Phone.Controls;
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


namespace Weekend {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image Background;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Pivot Pivot;
        
        internal FstnUserControl.CoinFlipCounter coinFlip;
        
        internal System.Windows.Controls.TextBlock resultText;
        
        internal Microsoft.Phone.Controls.PivotItem SettingsPI;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement daysOfweekT;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement daysOfweekCT;
        
        internal Microsoft.Phone.Controls.ListPicker daysOfweek;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement endOfWeekCT;
        
        internal Microsoft.Phone.Controls.TimePicker endOfWeek;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement endOfWeekT;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement weekendDurationT;
        
        internal FstnUserControl.Tile.TileSimpleVoidElement weekendDurationCT;
        
        internal Microsoft.Phone.Controls.ListPicker weekendDuration;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Weekend;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Background = ((System.Windows.Controls.Image)(this.FindName("Background")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Pivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Pivot")));
            this.coinFlip = ((FstnUserControl.CoinFlipCounter)(this.FindName("coinFlip")));
            this.resultText = ((System.Windows.Controls.TextBlock)(this.FindName("resultText")));
            this.SettingsPI = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("SettingsPI")));
            this.daysOfweekT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("daysOfweekT")));
            this.daysOfweekCT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("daysOfweekCT")));
            this.daysOfweek = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("daysOfweek")));
            this.endOfWeekCT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("endOfWeekCT")));
            this.endOfWeek = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("endOfWeek")));
            this.endOfWeekT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("endOfWeekT")));
            this.weekendDurationT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("weekendDurationT")));
            this.weekendDurationCT = ((FstnUserControl.Tile.TileSimpleVoidElement)(this.FindName("weekendDurationCT")));
            this.weekendDuration = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("weekendDuration")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
        }
    }
}

