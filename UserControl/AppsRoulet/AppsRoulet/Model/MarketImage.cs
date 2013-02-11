using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public partial class MarketImage
    {
        public string toString()
        {
            String retour = "image: Id:" + Id;
            return retour;
        }
    }
}
