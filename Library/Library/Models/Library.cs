using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Models
{
    public class Library
    {
        public enum TransactionState
        {
            PERSON_NOT_FOUND,
            BOOK_NOT_FOUND,
            BOOK_NOT_AVAILABLE,
            TRANSACTION_OK,
        }
        List<Person> people;
        List<Book> books;
        public Library()
        {
            people = Person.CreateTestData();
            //books = Book.CreateTestData();
        }
        public void AddBook(string _iSBN, string _title, string _author)
        {
            books.Add(new Book(_iSBN,_title,_author));
        }
        public Person GetPerson(int _id)
        {
            return people.FirstOrDefault(p => p.Id == _id);
        }
        public Book GetBook(string _iSBN)
        {

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].ISBN == _iSBN)
                    return books[i];
            }

            //LINQ

            //return (from b in books
            //        where b.ISBN == _iSBN
            //        select b).FirstOrDefault();

            

            return books.FirstOrDefault(b => b.ISBN == _iSBN);
        }
        public List<Book> SearchBook(string query)
        {

            //var result = books.Where(b => b.Author.Contains(query) || b.Title.Contains(query) || b.ISBN.Contains(query)).Any();
          
            return books.Where(b => b.Author.Contains(query) || b.Title.Contains(query) || b.ISBN.Contains(query)).ToList();
        }

        public TransactionState Borrow(int _idPerson, string _iSBN)
        {
            var bookToBorrow = books.FirstOrDefault(b => b.ISBN == _iSBN);
            var person = people.FirstOrDefault(p => p.Id == _idPerson);
            if (bookToBorrow == null)
                return TransactionState.BOOK_NOT_FOUND;
            if (person == null)
                return TransactionState.PERSON_NOT_FOUND;
            if (!bookToBorrow.IsAvailable)
                return TransactionState.BOOK_NOT_AVAILABLE;

            bookToBorrow.IsAvailable = false;
            person.Books.Add(bookToBorrow);
            if (bookToBorrow.OnBookStatusChanged != null)
                bookToBorrow.OnBookStatusChanged(person);

            return TransactionState.TRANSACTION_OK;
        }
        public TransactionState Return(int _idPerson, string _iSBN)
        {
            var bookToBorrow = books.FirstOrDefault(b => b.ISBN == _iSBN);
            var person = people.FirstOrDefault(p => p.Id == _idPerson);
            if (bookToBorrow == null)
                return TransactionState.BOOK_NOT_FOUND;
            if (person == null)
                return TransactionState.PERSON_NOT_FOUND;
            if (!bookToBorrow.IsAvailable)
                return TransactionState.BOOK_NOT_AVAILABLE;

            bookToBorrow.IsAvailable = true;
            person.Books.Remove(bookToBorrow);

            if (bookToBorrow.OnBookStatusChanged != null)
                bookToBorrow.OnBookStatusChanged(person);

            return TransactionState.TRANSACTION_OK;
        }
    }
}
