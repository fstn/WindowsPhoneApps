using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnUserControl.Apps.Model
{
    public class MarketCat
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public MarketCat()
        {

        }
        public MarketCat(String Id,String Title)
        {
            this.Title = Title;
            this.Id = Id;
        }
    }
}
