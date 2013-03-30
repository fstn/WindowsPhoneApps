using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using AppsRoulet.Model;
using Microsoft.Phone.Controls;
using AppsRoulet.util;

namespace AppsRoulet
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MyDataContext db;
        private XMLToBDDBatch xmlBatch;
        private Queue<Uri> CatsToDo;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
          

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CatsToDo = new Queue<Uri>(); 
            db = MyDataContextFactory.Instance.DataContext;
            if (db.DatabaseExists())
            {
                db.DeleteDatabase();
            }
            db.CreateDatabase();
            XMLCatParser xmlCatParser = new XMLCatParser(CatsToDo);
            xmlCatParser.load(URIModel.getInstance().getMarketCatsUri());
            xmlCatParser.Completed += xmlCatParser_Completed;
           

        }

        void xmlCatParser_Completed(object sender)
        {
            Debugger.Log(1, "ok", "done");
            xmlBatch = new XMLToBDDBatch();
            while (CatsToDo.Count > 0)
            {
                xmlBatch.addUri(new Uri(CatsToDo.Dequeue().AbsoluteUri + "apps/?chunksize=20"));
            }
            xmlBatch.start();
        }

    }
}