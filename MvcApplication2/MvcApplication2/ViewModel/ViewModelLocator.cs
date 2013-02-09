using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.ViewModel
{
    public static class ViewModelLocator
    {
        private static CreateViewModel createViewModel=new CreateViewModel();

        public static CreateViewModel CreateViewModel
        {
            get { return createViewModel; }
            set { createViewModel = value; }
        }
        

    }
}