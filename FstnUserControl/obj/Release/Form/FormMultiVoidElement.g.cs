﻿#pragma checksum "C:\DevTools\Git\windowsPhone\FstnUserControl\Form\FormMultiVoidElement.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B330EE31B5143FFAAD291C3E90EB1A12"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FstnUserControl.Form;
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
    
    
    public partial class FormMultiVoidElement : FstnUserControl.Form.AbstractForm {
        
        internal FstnUserControl.Form.AbstractForm userControl;
        
        internal System.Windows.Controls.StackPanel LayoutRoot;
        
        internal System.Windows.Controls.TextBlock Label;
        
        internal System.Windows.Controls.StackPanel SecondLine;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/FstnUserControl;component/Form/FormMultiVoidElement.xaml", System.UriKind.Relative));
            this.userControl = ((FstnUserControl.Form.AbstractForm)(this.FindName("userControl")));
            this.LayoutRoot = ((System.Windows.Controls.StackPanel)(this.FindName("LayoutRoot")));
            this.Label = ((System.Windows.Controls.TextBlock)(this.FindName("Label")));
            this.SecondLine = ((System.Windows.Controls.StackPanel)(this.FindName("SecondLine")));
        }
    }
}

