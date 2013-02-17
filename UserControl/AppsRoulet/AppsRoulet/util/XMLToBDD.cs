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
    public class XMLToBDD
    {
        private List<MarketApp> listOfApps;
        private MyDataContext dt;
        public event XMLToBDDCompletedEventHandler Completed;
        public XMLToBDD(MyDataContext dt)
        {
            this.dt = dt;
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

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            String atom = "{http://www.w3.org/2005/Atom}";
            String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
            XDocument doc = XDocument.Parse(e.Result);
            listOfApps = (from c in doc.Descendants(atom + "entry")
                          select new MarketApp()
                          {
                              Id = c.Element(atom + "id").Value,
                              Title = c.Element(atom + "title").Value,
                              SortTitle = c.Element(defaultXlmns + "sortTitle").Value,
                              Update = Convert.ToDateTime(c.Element(atom + "updated").Value),
                              ReleaseDate = Convert.ToDateTime(c.Element(defaultXlmns + "releaseDate").Value),
                              Version = c.Element(defaultXlmns + "version").Value,
                              AverageUserRating = Convert.ToDouble(c.Element(defaultXlmns + "averageUserRating").Value),
                              UserRatingCount = Convert.ToInt16(c.Element(defaultXlmns + "userRatingCount").Value),
                              Image = new MarketImage()
                              {
                                  Id = c.Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value
                              },
                              CategoriesList = (from c2 in c.Element(defaultXlmns + "categories").Descendants(defaultXlmns + "category")
                                                select new MarketCat()
                                                {
                                                    Id = c2.Element(defaultXlmns + "id").Value,
                                                    Title = c2.Element(defaultXlmns + "title").Value
                                                }).ToList()/*,
                              OffersList = (from c3 in c.Element(defaultXlmns + "offers").Descendants(defaultXlmns + "offer")
                                            select new MarketOffer()
                                            {
                                                MediaInstanceId = c3.Element(defaultXlmns + "mediaInstanceId").Value,
                                                Id = c3.Element(defaultXlmns + "offerId").Value,
                                                Price = Convert.ToDouble(c3.Element(defaultXlmns + "price").Value),
                                                PriceCurrencyCode = c3.Element(defaultXlmns + "priceCurrencyCode").Value
                                            }
                                  ).ToList()*/
                          }).ToList<MarketApp>();

            foreach (MarketApp app in listOfApps)
            {
              //  try
              //  {
                    dt.MarketApps.InsertOnSubmit(app);
                    //dt.Log = new BDDLogger();
                    dt.SubmitChanges();
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
