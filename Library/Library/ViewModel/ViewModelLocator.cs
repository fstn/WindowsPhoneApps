using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**
 * Va contenir une référence vers tous les viewModel
 * */
namespace Library.ViewModel
{
   
    public class ViewModelLocator
    {
        public ListOfBooksViewModel ListOfBooksViewModel { get; set; }
        public EditBookViewModel EditBookViewModel { get; set; }
        public AddBookViewModel AddBookViewModel { get; set; }
        public DetailsBookViewModel DetailsBookViewModel { get; set; }
        public ViewModelLocator()
        {
            //il faut instancier le model
            ListOfBooksViewModel = new ListOfBooksViewModel();
            EditBookViewModel = new EditBookViewModel();
            AddBookViewModel = new AddBookViewModel();
            DetailsBookViewModel = new DetailsBookViewModel();
        }
    }
}
