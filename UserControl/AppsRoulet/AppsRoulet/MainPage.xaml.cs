using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using AppsRoulet.Model;
using Microsoft.Phone.Controls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
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
            XMLCatParser xmlCatParser = new XMLCatParser(CatsToDo);
            xmlCatParser.load(URIModel.getInstance().getMarketCatsUri());
            xmlCatParser.Completed += xmlCatParser_Completed;
            db = MyDataContextFactory.Instance.DataContext;
            if (db.DatabaseExists())
            {
                db.DeleteDatabase();
            }
            db.CreateDatabase();

        }

        void xmlCatParser_Completed(object sender)
        {
            Debugger.Log(1, "ok", "done");
            xmlBatch = new XMLToBDDBatch(db);
            while (CatsToDo.Count > 0)
            {
                xmlBatch.addUri(new Uri(CatsToDo.Dequeue().AbsoluteUri + "apps/?chunksize=20"));
            }
            xmlBatch.start();
        }

    }
}