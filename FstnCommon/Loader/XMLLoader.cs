using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace FstnCommon.Loader
{
    public class XMLLoader : ILoader
    {
        public event LoaderLoadedEventHandler Loaded;
        public event LoaderErrorEventHandler Error;
        private Uri uri;
        public XMLLoader()
        {
        }

        public void load(Uri uri)
        {
            try
            {
                this.uri = uri;
                WebClient client = new WebClient();
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadStringAsync(uri);
                Debugger.Log(0, "XML", "xml  load " + uri.AbsoluteUri);
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
            try
            {
                XDocument doc = XDocument.Parse(e.Result);
                if (Loaded != null)
                {
                    Loaded(this, doc);
                }
            }
            catch (WebException we)
            {
                if (Error != null)
                {
                    Debugger.Log(1, "error", "can't connect to: " + uri.AbsoluteUri);
                    Error(this, we);
                }
            }
        }
    }
}
