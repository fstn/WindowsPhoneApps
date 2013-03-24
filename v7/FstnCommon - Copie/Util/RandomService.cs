using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnCommon.Util
{
    public class RandomService
    {

        private Random rand;
        #region Singleton

        private static RandomService _instance = null;

        private RandomService() { }

        public static RandomService Instance
        {
            get
            {
                if (_instance == null)
                {
                    RandomService instance = new RandomService();
                    instance.Initialize();
                    _instance = instance;
                }
                return _instance;
            }
        }

        #endregion

        private void Initialize()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }
        public int getRand(int min, int max)
        {
            return rand.Next(min, max);
        }
        public int getRand()
        {
            return rand.Next();
        }

        
	
    }
}
