using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public class MarketOffer
    {
        //offerId
        public String Id { get; set; }
        public String MediaInstanceId { get; set; }
        public double Price { get; set; }
        public String PriceCurrencyCode { get; set; }
        public  string toString()
        {
            String retour = "offer: Id:" + Id+" Price: "+Price ;
            return retour;
        }
    }

}
