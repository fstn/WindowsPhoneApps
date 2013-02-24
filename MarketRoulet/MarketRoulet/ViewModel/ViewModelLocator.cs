using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketRoulet.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel{get;set;}
        public ViewModelLocator (){
            MainViewModel = new MainViewModel();
        }
    }
}
