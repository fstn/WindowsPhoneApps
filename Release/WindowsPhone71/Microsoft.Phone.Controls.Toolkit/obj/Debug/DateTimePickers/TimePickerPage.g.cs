﻿#pragma checksum "C:\DevTools\Git\windowsPhone\Release\WindowsPhone71\Microsoft.Phone.Controls.Toolkit\DateTimePickers\TimePickerPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "783210E0EAB2DB23CBF42304D1339E84"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls.Primitives;
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


namespace Microsoft.Phone.Controls {
    
    
    public partial class TimePickerPage : Microsoft.Phone.Controls.Primitives.DateTimePickerPageBase {
        
        internal System.Windows.VisualStateGroup VisibilityStates;
        
        internal System.Windows.VisualState Open;
        
        internal System.Windows.VisualState Closed;
        
        internal System.Windows.Media.PlaneProjection PlaneProjection;
        
        internal System.Windows.Shapes.Rectangle SystemTrayPlaceholder;
        
        internal System.Windows.Controls.TextBlock HeaderTitle;
        
        internal Microsoft.Phone.Controls.Primitives.LoopingSelector PrimarySelector;
        
        internal Microsoft.Phone.Controls.Primitives.LoopingSelector SecondarySelector;
        
        internal Microsoft.Phone.Controls.Primitives.LoopingSelector TertiarySelector;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Microsoft.Phone.Controls.Toolkit;component/DateTimePickers/TimePickerPage.xaml", System.UriKind.Relative));
            this.VisibilityStates = ((System.Windows.VisualStateGroup)(this.FindName("VisibilityStates")));
            this.Open = ((System.Windows.VisualState)(this.FindName("Open")));
            this.Closed = ((System.Windows.VisualState)(this.FindName("Closed")));
            this.PlaneProjection = ((System.Windows.Media.PlaneProjection)(this.FindName("PlaneProjection")));
            this.SystemTrayPlaceholder = ((System.Windows.Shapes.Rectangle)(this.FindName("SystemTrayPlaceholder")));
            this.HeaderTitle = ((System.Windows.Controls.TextBlock)(this.FindName("HeaderTitle")));
            this.PrimarySelector = ((Microsoft.Phone.Controls.Primitives.LoopingSelector)(this.FindName("PrimarySelector")));
            this.SecondarySelector = ((Microsoft.Phone.Controls.Primitives.LoopingSelector)(this.FindName("SecondarySelector")));
            this.TertiarySelector = ((Microsoft.Phone.Controls.Primitives.LoopingSelector)(this.FindName("TertiarySelector")));
        }
    }
}

