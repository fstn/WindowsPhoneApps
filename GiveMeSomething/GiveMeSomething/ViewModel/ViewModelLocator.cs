using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiveMeSomething.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }
        public ViewModelLocator (){
            MainViewModel = new MainViewModel();
            SettingsViewModel = new SettingsViewModel();
        }
    }
}
