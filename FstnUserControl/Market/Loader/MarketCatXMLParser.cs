using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FstnCommon.Loader;
using FstnCommon.Market.Model;
using FstnCommon.Parser;

namespace FstnUserControl.MarketLoader
{
    public class MarketCatXMLParser :XMLParser
    {
          public MarketCatXMLParser()
        {
        }

          public override void parse(XDocument doc)
          {
              MarketCat retour = new MarketCat();
              String atom = "{http://www.w3.org/2005/Atom}";
              String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
              retour = new MarketCat()
              {
                  Id = doc.Element(atom + "entry").Element(atom + "id").Value,
                  Title = doc.Element(atom + "entry").Element(atom + "title").Value
              };
              OnComplete(retour);
        }
    }
}
