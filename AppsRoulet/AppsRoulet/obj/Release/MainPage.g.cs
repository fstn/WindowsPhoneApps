﻿#pragma checksum "C:\DevTools\Git\windowsPhone\AppsRoulet\AppsRoulet\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "857ADDEA66BC8F7AD97E614055CAC992"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace AppsRoulet {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard StoryBoard;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Canvas canvas;
        
        internal System.Windows.Controls.Image non;
        
        internal System.Windows.Controls.Image finalResponseImg;
        
        internal System.Windows.Controls.Image finalScreenImg;
        
        internal System.Windows.Controls.TextBlock resultText;
        
        internal Microsoft.Phone.Controls.TimePicker endOfWeek;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/AppsRoulet;component/MainPage.xaml", System.UriKind.Relative));
            this.StoryBoard = ((System.Windows.Media.Animation.Storyboard)(this.FindName("StoryBoard")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.canvas = ((System.Windows.Controls.Canvas)(this.FindName("canvas")));
            this.non = ((System.Windows.Controls.Image)(this.FindName("non")));
            this.finalResponseImg = ((System.Windows.Controls.Image)(this.FindName("finalResponseImg")));
            this.finalScreenImg = ((System.Windows.Controls.Image)(this.FindName("finalScreenImg")));
            this.resultText = ((System.Windows.Controls.TextBlock)(this.FindName("resultText")));
            this.endOfWeek = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("endOfWeek")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
        }
    }
}

