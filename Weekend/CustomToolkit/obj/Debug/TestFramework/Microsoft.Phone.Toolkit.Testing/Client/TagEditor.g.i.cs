﻿#pragma checksum "C:\DevTools\Git\windowsPhone\Weekend\CustomToolkit\TestFramework\Microsoft.Phone.Toolkit.Testing\Client\TagEditor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BEF34FEF3C5B7EDFAB71D42227D1DF2D"
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


namespace Microsoft.Phone.Testing.Client {
    
    
    public partial class TagEditor : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock UseTagsText;
        
        internal Microsoft.Phone.Controls.ToggleSwitch TagsToggle;
        
        internal System.Windows.Controls.Grid TagsPanel;
        
        internal System.Windows.Controls.TextBox TagExpressionTextBox;
        
        internal System.Windows.Controls.ListBox SuggestionsListBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/CustomToolkit;component/TestFramework/Microsoft.Phone.Toolkit.Testing/Client/Tag" +
                        "Editor.xaml", System.UriKind.Relative));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.UseTagsText = ((System.Windows.Controls.TextBlock)(this.FindName("UseTagsText")));
            this.TagsToggle = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("TagsToggle")));
            this.TagsPanel = ((System.Windows.Controls.Grid)(this.FindName("TagsPanel")));
            this.TagExpressionTextBox = ((System.Windows.Controls.TextBox)(this.FindName("TagExpressionTextBox")));
            this.SuggestionsListBox = ((System.Windows.Controls.ListBox)(this.FindName("SuggestionsListBox")));
        }
    }
}

