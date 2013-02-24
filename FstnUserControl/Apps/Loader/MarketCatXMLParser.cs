using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FstnCommon.Loader;
using FstnCommon.Parser;
using FstnUserControl.Apps.Model;

namespace FstnUserControl.Apps.Loader
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
