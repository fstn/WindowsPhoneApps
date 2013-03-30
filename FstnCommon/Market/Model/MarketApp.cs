using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnCommon.Market.Model
{
    public class MarketApp
    {
        public String Title { get; set; }
        public DateTime Update { get; set; }
        public int IdGen { get; set; }
        public String Id;
        public String SortTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public String Version { get; set; }
        public double AverageUserRating { get; set; }
       
        public int UserRatingCount { get; set; }
        public String Image { get; set; }
        public String LinkToMarket { get; set; }
        public String Description { get; set; }
        public String DownloadLink { get; set; }


        public static MarketApp Empty { get { return new MarketApp(); } }
        public MarketApp()
        { 
        }

    }
}
