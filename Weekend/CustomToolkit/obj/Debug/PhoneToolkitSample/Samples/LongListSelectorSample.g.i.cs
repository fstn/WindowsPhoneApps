﻿#pragma checksum "C:\DevTools\Git\windowsPhone\Weekend\CustomToolkit\PhoneToolkitSample\Samples\LongListSelectorSample.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A2131562B6DE13B0035EB6DEA1B96F12"
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


namespace PhoneToolkitSample.Samples {
    
    
    public partial class LongListSelectorSample : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.LongListSelector buddies;
        
        internal Microsoft.Phone.Controls.LongListSelector linqMovies;
        
        internal Microsoft.Phone.Controls.LongListSelector codeMovies;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/CustomToolkit;component/PhoneToolkitSample/Samples/LongListSelectorSample.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.buddies = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("buddies")));
            this.linqMovies = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("linqMovies")));
            this.codeMovies = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("codeMovies")));
        }
    }
}

