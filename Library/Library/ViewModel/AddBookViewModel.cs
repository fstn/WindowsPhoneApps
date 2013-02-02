using GalaSoft.MvvmLight.Command;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Library.ViewModel
{
    public class AddBookViewModel :ViewModelBase
    {
        public RelayCommand AddBook{get;set;}
        public String ISBN { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }


        public RelayCommand ViewLoadedCommand { get; set; }

        public AddBookViewModel()
        {
            AddBook = new RelayCommand(() =>
            {
                Book newBook =new Book(ISBN, Title, Author, false);
                ObservableCollection<Book>  LstBooks =
                   ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.LstBooks ;
                LstBooks.Add(newBook);
                ((ViewModelLocator)Application.Current.Resources["Locator"]).DetailsBookViewModel.CurrentBook = newBook;
                var rootFrame = (App.Current as App).RootFrame;
                rootFrame.Navigate(new Uri("/DetailsBook.xaml", UriKind.Relative));
                ISBN = "";
                Title = "";
                Author = "";
             });

            ViewLoadedCommand = new RelayCommand(() =>
                {
                });
        }
    }
}
