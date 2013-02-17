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
        private MyDataContext dt;
        public event XMLToBDDBatchCompletedEventHandler Completed;
        private XMLToBDD xmlLoader;
        public XMLToBDDBatch(MyDataContext dt)
        {
            this.dt = dt;
             xmlLoader = new XMLToBDD(dt);
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
