﻿#pragma checksum "C:\DevTools\Git\windowsPhone\GiveMeSomething\GiveMeSomething\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A3901056C6C0748CCC4AC94E7AFEA879"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FstnUserControl.Error;
using Microsoft.Advertising.Mobile.UI;
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


namespace GiveMeSomething {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Image Background;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Advertising.Mobile.UI.AdControl AdControl;
        
        internal System.Windows.Controls.StackPanel ContentLayout;
        
        internal System.Windows.Controls.TextBlock CategorieTitle;
        
        internal System.Windows.Controls.StackPanel AppsPreviewContent;
        
        internal FstnUserControl.Error.ErrorDisplayer ErrorDisplayer;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GiveMeSomething;component/MainPage.xaml", System.UriKind.Relative));
            this.Background = ((System.Windows.Controls.Image)(this.FindName("Background")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.AdControl = ((Microsoft.Advertising.Mobile.UI.AdControl)(this.FindName("AdControl")));
            this.ContentLayout = ((System.Windows.Controls.StackPanel)(this.FindName("ContentLayout")));
            this.CategorieTitle = ((System.Windows.Controls.TextBlock)(this.FindName("CategorieTitle")));
            this.AppsPreviewContent = ((System.Windows.Controls.StackPanel)(this.FindName("AppsPreviewContent")));
            this.ErrorDisplayer = ((FstnUserControl.Error.ErrorDisplayer)(this.FindName("ErrorDisplayer")));
        }
    }
}
