﻿#pragma checksum "C:\DevTools\Git\windowsPhone\brokeThePig\BrokeThePig\BrokeThePig\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C207DD58EF3CFE5F2412EB6687DADD56"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BrokeThePig.UC;
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


namespace BrokeThePig {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Advertising.Mobile.UI.AdControl AdControl;
        
        internal System.Windows.Controls.Canvas ContentLayout;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock Counter;
        
        internal System.Windows.Controls.TextBlock AmountOfMoney;
        
        internal System.Windows.Controls.Canvas PigLayout;
        
        internal BrokeThePig.UC.Pig Pig;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/BrokeThePig;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.AdControl = ((Microsoft.Advertising.Mobile.UI.AdControl)(this.FindName("AdControl")));
            this.ContentLayout = ((System.Windows.Controls.Canvas)(this.FindName("ContentLayout")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.Counter = ((System.Windows.Controls.TextBlock)(this.FindName("Counter")));
            this.AmountOfMoney = ((System.Windows.Controls.TextBlock)(this.FindName("AmountOfMoney")));
            this.PigLayout = ((System.Windows.Controls.Canvas)(this.FindName("PigLayout")));
            this.Pig = ((BrokeThePig.UC.Pig)(this.FindName("Pig")));
        }
    }
}

