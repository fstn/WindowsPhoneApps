﻿#pragma checksum "C:\Users\a-gucarl\Desktop\mai-WP8 - RG2013\BeJournalist\Admin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3AD60684DA6661124D34FB62162F80AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.18010
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
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


namespace BeJournalist {
    
    
    public partial class Admin : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox TextComment;
        
        internal System.Windows.Controls.Button Badd;
        
        internal Microsoft.Phone.Controls.LongListSelector ListItems;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/BeJournalist;component/Admin.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TextComment = ((System.Windows.Controls.TextBox)(this.FindName("TextComment")));
            this.Badd = ((System.Windows.Controls.Button)(this.FindName("Badd")));
            this.ListItems = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("ListItems")));
        }
    }
}

