using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Models
{
    public class Person
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public List<Book> Books{ get; set; }

        public Person( int _id, string _firstName, string _lastName)
        {
            Id = _id;
            FirstName = _firstName;
            LastName = _lastName;
        }
        public override string ToString()
        {
            return string.Format("Id: {0} First Name: {1} LastName: {2}",Id,FirstName,LastName);
        }

        public static List<Person> CreateTestData(int count = 5)
        {
            List<Person> lstPeople = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                lstPeople.Add(new Person(i,"name" + i,"lastname" + i));
            }
            return lstPeople;
        }
    }
}
