﻿#pragma checksum "C:\DevTools\Git\windowsPhone\FstnUserControl\Apps\AppsPreview.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "41C5BEC2F38554050ABF52873D6B98F0"
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


namespace FstnUserControl {
    
    
    public partial class AppsPreview : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas RootLayout;
        
        internal System.Windows.Controls.StackPanel ContentLayout;
        
        internal System.Windows.Controls.TextBlock Title;
        
        internal System.Windows.Controls.Image Image;
        
        internal Microsoft.Phone.Controls.Rating RatingControl;
        
        internal System.Windows.Controls.TextBlock Description;
        
        internal FstnUserControl.ImageWaiting WaitingAnim;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/FstnUserControl;component/Apps/AppsPreview.xaml", System.UriKind.Relative));
            this.RootLayout = ((System.Windows.Controls.Canvas)(this.FindName("RootLayout")));
            this.ContentLayout = ((System.Windows.Controls.StackPanel)(this.FindName("ContentLayout")));
            this.Title = ((System.Windows.Controls.TextBlock)(this.FindName("Title")));
            this.Image = ((System.Windows.Controls.Image)(this.FindName("Image")));
            this.RatingControl = ((Microsoft.Phone.Controls.Rating)(this.FindName("RatingControl")));
            this.Description = ((System.Windows.Controls.TextBlock)(this.FindName("Description")));
            this.WaitingAnim = ((FstnUserControl.ImageWaiting)(this.FindName("WaitingAnim")));
        }
    }
}

