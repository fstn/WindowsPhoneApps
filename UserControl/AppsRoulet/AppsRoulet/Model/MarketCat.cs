using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public class MarketCat
    {
        public String Id { get; set; }
        public String Title { get; set; }
         public  string toString()
        {
            String retour="category: TItle:"+Title+", Id:"+Id;
            return retour;
        }
    }
}
