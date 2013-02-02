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
    public class DetailsBookViewModel:ViewModelBase
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
                    rootFrame.Navigate(new Uri("/Error.xaml", UriKind.Relative));
                }
            }
        }
        
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
            


        public DetailsBookViewModel()
        {

        }
    }
}
