using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BrokeThePig
{
    public partial class Congratulation : PhoneApplicationPage
    {
        public Congratulation()
        {
            InitializeComponent();
            this.Tap += Congratulation_Tap;
        }

        void Congratulation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BrokeThePig;component/MainPage.xaml?reset=1", UriKind.Relative));
        }
    }
}