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
    public class MarketAppXMLParser :XMLParser
    {
        public event ParserCompleteEventHandler Completed;
          public MarketAppXMLParser()
        {
        }

        public override void parse(XDocument doc)
          {
              MarketApp retour = new MarketApp();
              String atom = "{http://www.w3.org/2005/Atom}";
              String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
              retour = new MarketApp()
              {
                  Id = doc.Element(atom + "entry").Element(atom + "id").Value.Replace("urn:uuid:", String.Empty),
                  Title = doc.Element(atom + "entry").Element(atom + "title").Value,
                  Description = doc.Element(atom + "entry").Element(atom + "content")!=null?doc.Element(atom + "entry").Element(atom + "content").Value:"",
                  SortTitle = doc.Element(atom + "entry").Element(defaultXlmns + "sortTitle").Value,
                  Image = doc.Element(atom + "entry").Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value.Replace("urn:uuid:", String.Empty)
              };
              retour.Image = URIModel.Instance.getImageUriString() + retour.Image;
              OnComplete(retour);
        }
    }
}
