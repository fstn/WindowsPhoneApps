using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AppsRoulet.Model;

namespace AppsRoulet.util
{
    public delegate void CompletedEventHandler(object sender);
    public class XMLCatParser
    {
        public event CompletedEventHandler Completed;
        private List<MarketCat> listOfCats;
        private Queue<Uri> uriToDo;
        private int toLoad = 0;
        private int loaded = 0;

        public XMLCatParser(Queue<Uri> uriToDo)
        {
            this.uriToDo = uriToDo;
        }
        public void load(Uri uri)
        {
            toLoad++;
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            WebClient client = new WebClient();
            client.DownloadStringAsync(uri);
            client.DownloadStringCompleted += client_DownloadStringCompleted;
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            loaded++;
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXmlns ="{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            listOfCats = (from c in doc.Descendants(atom + "entry")
                          select new MarketCat()
                          {
                              Id = c.Element(atom + "id").Value,
                              Title = c.Element(atom + "title").Value,
                              ParentId = (c.Element(defaultXmlns + "parentId") != null ? (c.Element(defaultXmlns + "parentId")).Value : null)
                          }).ToList();
            foreach (MarketCat cat in listOfCats)
            {
                Uri uri = new Uri(URIModel.getInstance().getMarketCatsUri() + cat.Id + "/");
                uriToDo.Enqueue(uri);
                MyDataContextFactory.Instance.DataContext.MarketCategories.InsertOnSubmit(cat);
                MyDataContextFactory.Instance.DataContext.SubmitChanges();
                if (cat.ParentId == null)
                {
                    load(uri);
                }
            }
            if (loaded == toLoad)
            {
                if (Completed != null)
                    Completed(this);
            }
        }
    }
}
