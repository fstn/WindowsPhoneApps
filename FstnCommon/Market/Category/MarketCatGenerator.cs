using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FstnCommon.Market.Model;

namespace FstnCommon.Market.Category
{
    public class MarketCatGenerator
    {


        #region Singleton

        private static MarketCatGenerator _instance = null;

        private MarketCatGenerator() { }

        public static MarketCatGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    MarketCatGenerator instance = new MarketCatGenerator();
                    instance.Initialize();
                    _instance = instance;
                }
                return _instance;
            }
        }

        #endregion


        public List<MarketCat> Categories { get; set; }
        private void Initialize()
        {
            Categories = new List<MarketCat>();
            Categories.Add(new MarketCat("windowsphone.Best", "Top"));
            Categories.Add(new MarketCat("windowsphone.Games", "Games"));
            Categories.Add(new MarketCat("windowsphone.MusicAndVideo", "Music Video"));
            Categories.Add(new MarketCat("windowsphone.NewsAndWeather", "News"));
            Categories.Add(new MarketCat("windowsphone.Photo", "Photo"));
            Categories.Add(new MarketCat("windowsphone.Social", "Social"));
            Categories.Add(new MarketCat("windowsphone.Sports", "Sports"));
            Categories.Add(new MarketCat("windowsphone.ToolsAndProductivity", "Tools"));
        }

        
	
    }
}
