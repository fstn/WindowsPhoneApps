using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppsRoulet.Model;

namespace AppsRoulet.util
{
    public delegate void XMLToBDDBatchCompletedEventHandler(object sender);
    public class XMLToBDDBatch
    {
        public Queue<Uri> ToDownload { get; set; }
        private MyDataContext db;
        private XMLToBDDApp xmlLoader;
        public event XMLToBDDBatchCompletedEventHandler Completed;
        public XMLToBDDBatch()
        {
            this.db = MyDataContextFactory.Instance.DataContext;
             xmlLoader = new XMLToBDDApp();
             xmlLoader.Completed += start;
             ToDownload = new Queue<Uri>();
        }


        public void addUri(Uri uri)
        {
            ToDownload.Enqueue(uri);
        }

        public void start(object e=null)
        {
            if (ToDownload.Count > 0)
            {
                Uri uri = ToDownload.Dequeue(); 
                xmlLoader.load(uri);
            }
            else
            {
                if (Completed != null)
                    Completed(this);

            }
        }
    }
}
