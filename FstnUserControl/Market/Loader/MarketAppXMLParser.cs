using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
using System.Xml.Linq;
using FstnCommon.Loader;
using FstnCommon.Market.Model;
using FstnCommon.Parser;
using FstnCommon.Util;
using FstnUserControl.Error;

namespace FstnUserControl.Market.Loader
{
    public class MarketAppXMLParser :XMLParser
    {
        public MarketApp parsedApp;
        public MarketApp ParsedApp
        {
            get
            {
                return parsedApp;
            }
        }
          public MarketAppXMLParser()
        {
        }

        public override void parse(XDocument doc)
          {
              try {
                  parsedApp = new MarketApp();
                  String atom = "{http://www.w3.org/2005/Atom}";
                  String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
                  parsedApp = new MarketApp()
                  {
                      Id = doc.Element(atom + "entry").Element(atom + "id").Value.Replace("urn:uuid:", String.Empty),
                      Title = doc.Element(atom + "entry").Element(atom + "title").Value,
                      Description = doc.Element(atom + "entry").Element(atom + "content") != null ? doc.Element(atom + "entry").Element(atom + "content").Value : "",
                      SortTitle = doc.Element(atom + "entry").Element(defaultXlmns + "sortTitle").Value,
                      Image = doc.Element(atom + "entry").Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value.Replace("urn:uuid:", String.Empty)
                  };
                  parsedApp.Image = "http://cdn.marketplaceimages.windowsphone.com/v3.2/en-US/image/" + parsedApp.Image;
                  OnComplete(parsedApp);
              }catch(NullReferenceException ne){
                  OnError(ne);
                  if (doc == null)
                      ErrorService.Instance.AddError(this, "", ErrorType.NETWORKING_PROBLEM, ne);                  
              }
        }
    }
}
