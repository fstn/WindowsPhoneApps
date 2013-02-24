using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AppsRoulet.Model;

namespace AppsRoulet.util
{
    public delegate void XMLToBDDCompletedEventHandler(object sender);
    public abstract class XMLToBDD
    {
        protected MyDataContext db;
        public XMLToBDD()
        {
            this.db = MyDataContextFactory.Instance.DataContext;
        }
        public void load(Uri uri)
        {
            Debugger.Log(1, "info", uri.AbsoluteUri+"\n");
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            WebClient client = new WebClient();
            client.DownloadStringAsync(uri);
            client.DownloadStringCompleted += client_DownloadStringCompleted;
        }

        protected abstract void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e);
    }
}
