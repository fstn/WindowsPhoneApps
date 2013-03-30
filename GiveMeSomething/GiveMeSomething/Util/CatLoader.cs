using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiveMeSomething.Util
{
    public class CatLoader
    {
        public static CatLoader Instance { get; set; }
        static CatLoader()
        {
            Instance = new CatLoader();
        }

    }
}
