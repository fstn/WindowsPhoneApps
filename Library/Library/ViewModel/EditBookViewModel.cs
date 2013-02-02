using GalaSoft.MvvmLight.Command;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace Library.ViewModel
{
    public class EditBookViewModel:ViewModelBase
    {
        private Book currentBook;

        public Book CurrentBook
        {
            get { return currentBook; }
            set
            {
                currentBook = value;
                if (currentBook != null)
                {
                    ISBN = currentBook.ISBN;
                    Title = currentBook.Title;
                    author = currentBook.Author;
                }
                else
                {
                    var rootFrame = (App.Current as App).RootFrame;
                    rootFrame.Navigate(new Uri("/ListOfBooks.xaml", UriKind.Relative));
                }
            }
        }
        
        public RelayCommand ModifyBook{get;set;}
        public String ISBN { get; set; }

        private String title;

        public String Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private String author;

        public String Author
        {
            get { return author; }
            set { author = value;
            OnPropertyChanged("Author");
            }
        }
            


        public EditBookViewModel()
        {

            ModifyBook = new RelayCommand(() =>
            {
                var rootFrame = (App.Current as App).RootFrame;
                if (currentBook != null)
                {
                    currentBook.Author = Author;
                    currentBook.Title = Title;
                    currentBook.ISBN = ISBN;
                    ((ViewModelLocator)Application.Current.Resources["Locator"]).ListOfBooksViewModel.SelectedBook = null;
                    rootFrame.Navigate(new Uri("/ListOfBooks.xaml", UriKind.Relative));
                }
                else
                {

                    rootFrame.Navigate(new Uri("/Error.xaml", UriKind.Relative));
                }
            });
        }
    }
}
