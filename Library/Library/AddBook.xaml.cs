using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Library
{
    public partial class AddBook : PhoneApplicationPage
    {
        public AddBook()
        {
            InitializeComponent();
        }
        private void Home_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ListOfBooks.xaml", UriKind.Relative));

        }
    }
}