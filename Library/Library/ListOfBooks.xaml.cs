using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Library.ViewModel;

namespace Library
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        
        private void Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddBook.xaml",UriKind.Relative));
        }

        private void Home_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ListOfBooks.xaml", UriKind.Relative));
        }

        private void BookToEditSelected(object sender, RoutedEventArgs e)
        {

            if (((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook != ((ViewModelLocator)Application.Current.Resources["Locator"]).EditBookViewModel.CurrentBook)
            {
                ((ViewModelLocator)Application.Current.Resources["Locator"]).EditBookViewModel.CurrentBook = ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook;
            
                NavigationService.Navigate(new Uri("/EditBook.xaml", UriKind.Relative));
            }
        }

        private void ClicToShowDetailSelected(object sender, RoutedEventArgs e)
        {
            if (((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook != ((ViewModelLocator)Application.Current.Resources["Locator"]).DetailsBookViewModel.CurrentBook)
            {
                ((ViewModelLocator)Application.Current.Resources["Locator"]).DetailsBookViewModel.CurrentBook = ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook;
                NavigationService.Navigate(new Uri("/DetailsBook.xaml", UriKind.Relative));
            }

        }

    }
}