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
    public class XMLToBDDCat : XMLToBDD
    {
        public event XMLToBDDBatchCompletedEventHandler Completed;
        public List<MarketCat> listOfCats;
        protected override void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            listOfCats = (from c in doc.Descendants(atom + "entry")
                          select new MarketCat()
                          {
                              Id = c.Element(defaultXlmns + "id").Value,
                              Title = c.Element(defaultXlmns + "title").Value

                          }).ToList<MarketCat>();

            foreach (MarketCat cat in listOfCats)
            {
                db.MarketCategories.InsertOnSubmit(cat);
                db.Log = new BDDLogger();
                db.SubmitChanges();
            }
            if (Completed != null)
                Completed(this);
        }
    }
}
