using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models.Generated;

namespace MvcApplication2.ViewModel
{
    public class CreateViewModel
    {
        public Dictionary<int,Author> ListOfAuthors { get; set; }
        public int SelectedAuthorId { get; set; }
        public CreateViewModel()
        {
            Database1Entities context=new Database1Entities();
            ListOfAuthors = new Dictionary<int, Author>();
            foreach (Author author in (from m in context.Author select m).ToList())
            {
                ListOfAuthors.Add(author.Id, author);
            }

        }
    }
}