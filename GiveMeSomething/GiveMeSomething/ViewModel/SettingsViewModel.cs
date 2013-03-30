using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using GiveMeSomething.Resources;
using FstnUserControl;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using FstnCommon;
using System.Net;
using GiveMeSomething.Util;
using System.Xml.Linq;
using FstnCommon.Loader;
using FstnCommon.Parser;
namespace GiveMeSomething.ViewModel
{

    public class SettingsViewModel : ViewModelBase
    {
        #region prop
        #endregion
        public SettingsViewModel()
        {
            ListOfCounts=new List<int>();
            ListOfCounts.Add(10);
            ListOfCounts.Add(50);
            ListOfCounts.Add(100);
            ListOfCounts.Add(1000);
            ListOfCounts.Add(10000);
        }
        public List<int> ListOfCounts { get; set; }
        public int SelectedCount { get; set; }
    }
}
