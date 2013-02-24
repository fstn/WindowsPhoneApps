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
    public class XMLToBDDApp:XMLToBDD
    {
        public event XMLToBDDBatchCompletedEventHandler Completed;
        protected List<MarketApp> listOfApps;
        protected override void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            MyDataContext db=MyDataContextFactory.Instance.DataContext;
            listOfApps = (from c in doc.Descendants(atom + "entry")
                          select new MarketApp()
                          {
                              Id = c.Element(atom + "id").Value,
                              Title = c.Element(atom + "title").Value,
                              SortTitle = c.Element(defaultXlmns + "sortTitle").Value,
                              Update = Convert.ToDateTime(c.Element(atom + "updated").Value),
                              ReleaseDate = Convert.ToDateTime(c.Element(defaultXlmns + "releaseDate").Value),
                              Version = c.Element(defaultXlmns + "version").Value,
                              AverageUserRatingString = (c.Element(defaultXlmns + "averageUserRating").Value),
                              UserRatingCountString =(c.Element(defaultXlmns + "userRatingCount").Value),
                              Image=  c.Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value,
                              Categorie = (from c2 in c.Element(defaultXlmns + "categories").Descendants(defaultXlmns + "category")
                                             select (c2.Element(defaultXlmns + "id").Value)).First(),

                              PriceString = (from c2 in c.Element(defaultXlmns + "offers").Descendants(defaultXlmns + "offer")
                                       select c2.Element(defaultXlmns + "price").Value).First()
                            
                          }).ToList<Marke
            .tApp>();

            foreach (MarketApp app in listOfApps)
            {
                //  try
                //  {
                db.MarketApps.InsertOnSubmit(app);
                //dt.Log = new BDDLogger();
                db.SubmitChanges();
                //     }
                //    catch (Exception ee)
                //    {
                //       Debugger.Log(3, "sql", "\n"+ee.Message+"\n"+ee.StackTrace);
                //   }
                Debugger.Log(1, "info", app.toString() + "\n");
            }
            if (Completed != null)
                Completed(this);
        }
    }
}
