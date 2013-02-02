using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Library.Models
{
    public class Book
    {
        public delegate void BookStatusChanged(Person person);
        public BookStatusChanged OnBookStatusChanged;

        public string ISBN { get;  set; }
        public string Title { get;  set; }
        public string Author { get;  set; }
        public bool IsAvailable { get; set; }

        public Book( string _iSBN, string _title, string _author, bool _isAvailable = true)
        {
            ISBN = _iSBN;
            Title = _title;
            Author = _author;
            IsAvailable = _isAvailable;
        }
        public override string ToString()
        {
            return  IsAvailable? 
                string.Format("ISBN: {0} Title: {1} Author: {2} is available",ISBN,Title,Author)
                : string.Format("ISBN: {0} Title: {1} Author: {2} is NOT available") ;
        }

        public static ObservableCollection<Book> CreateTestData(int count = 5)
        {
            ObservableCollection<Book> lstBooks = new ObservableCollection<Book>();
            for (int i = 0; i < count; i++)
            {
                lstBooks.Add(new Book("isbn"+ i, "title" + i, "james" + i));
            }
            return lstBooks;
        }
    }
}
