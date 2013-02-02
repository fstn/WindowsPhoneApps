using Library.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;

namespace Library.ViewModel
{
    public class ListOfBooksViewModel : ViewModelBase
    {

        public RelayCommand<Book> ShowEditBook { get; set; }
        public RelayCommand<Book> ShowDetailBook { get; set; }
        #region Books

        private Book selectedBook;
        public Book SelectedBook
        {
            get
            {
                return this.selectedBook;
            }
            set
            {
                if (this.selectedBook != value)
                {
                    this.selectedBook = value;
                    this.OnPropertyChanged("SelectedBook");
                }
            }
        }

        private ObservableCollection<Book> lstBooks;
        public ObservableCollection<Book> LstBooks
        {
            get
            {
                if (this.lstBooks == null)
                    this.lstBooks = Book.CreateTestData();
                return this.lstBooks;
            }
            set
            {
                if (this.lstBooks != value)
                {
                    this.lstBooks = value;
                    this.OnPropertyChanged("LstBooks");
                }
            }
        }
        #endregion

        public ListOfBooksViewModel()
        {
            ShowDetailBook = new RelayCommand<Book>((Book book) =>
                {
                    ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook = book;
                  
                        ((ViewModelLocator)Application.Current.Resources["Locator"]).DetailsBookViewModel.CurrentBook = ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook;

                        var rootFrame = (App.Current as App).RootFrame;
                        rootFrame.Navigate(new Uri("/DetailsBook.xaml", UriKind.Relative));
                   
                });
            ShowEditBook = new RelayCommand<Book>((Book book) =>
            {
                ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook = book;

                    ((ViewModelLocator)Application.Current.Resources["Locator"]).EditBookViewModel.CurrentBook = ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook;
                    var rootFrame = (App.Current as App).RootFrame;
                    rootFrame.Navigate(new Uri("/EditBook.xaml", UriKind.Relative));

            });
        }
    }
}
