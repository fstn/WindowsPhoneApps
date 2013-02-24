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
    public class MarketAppXMLParserFromMP :XMLParser
    {
        public event ParserCompleteEventHandler Completed;
        public MarketAppXMLParserFromMP()
        {
        }

        public override void parse(XDocument doc)
          {
              MarketApp retour = new MarketApp();
              String atom = "{http://www.w3.org/2005/Atom}";
              String defaultXlmns = "{http://schemas.zune.net/catalog/apps/2008/02}";
              retour = new MarketApp()
              {
                  Id = doc.Element(atom + "feed").Element(atom + "id").Value,
                  Title = doc.Element(atom + "feed").Element(atom + "title").Value,
                  Description = doc.Element(atom + "feed").Element(atom + "content").Value,
                  Image = doc.Element(atom + "feed").Element(defaultXlmns + "image").Element(defaultXlmns + "id").Value,
                  DownloadLink = doc.Element(atom + "feed").Element(atom + "entry").Element(defaultXlmns + "url").Value,
                  AverageUserRating = Convert.ToDouble(doc.Element(atom + "feed").Element(defaultXlmns + "averageUserRating").Value.Replace('.',',')),      
                  UserRatingCount = Convert.ToInt16(doc.Element(atom + "feed").Element(defaultXlmns + "userRatingCount").Value)            
              };
              retour.Id = retour.Id.Replace("urn:uuid:", String.Empty);
              retour.Image = retour.Image.Replace("urn:uuid:", String.Empty);
              retour.Image = URIModel.Instance.getImageUriString() + retour.Image;
              OnComplete(retour);
        }
    }
}
