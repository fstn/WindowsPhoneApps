using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace FstnCommon.Loader
{
    public class XMLLoader:ILoader
    {
        public event LoaderLoadedEventHandler Loaded;
        public event LoaderErrorEventHandler Error;
        public XMLLoader()
        {
        }

        public void load(Uri uri)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadStringAsync(uri);
            }
            catch (WebException e)
            {
                if (Error != null)
                {
                    Error(this, e);
                }
            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XDocument doc = XDocument.Parse(e.Result);
            if (Loaded != null)
            {
                Loaded(this, doc);
            }
        }
    }
}
