
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using FstnDataBase.Tools;

namespace AppsRoulet.Model
{
    public partial class MarketApp
    {
        
      
        public List<String> ClientTypes { get; set; }
        public List<MarketCat> Categories { get; set; }
        public List<MarketOffer> Offers { get; set; }

      

        public MarketApp()
        {
            Init();
        }
        private void Init()
        {
            ClientTypes = new List<string>();
            Categories = new List<MarketCat>();
            Offers = new List<MarketOffer>();
        }

        public string toString()
        {
            String retour="application: Title:"+Title+", Id:"+Id;
                if( Categories !=null){
                    foreach (MarketCat cat in Categories){
                        retour += "\n \t"+cat.ToString();
                    }
                } 
                if (Offers != null)
                {
                    foreach (MarketOffer offer in Offers)
                    {
                        retour += "\n \t" + offer.ToString();
                    }
                }
                retour += "\n " + Image.toString();
            return retour;
        }

    }
}
