using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Ress
{
    public class ResourceWrapper
    {
        private static Ress.Resource localizedResources = new Ress.Resource();

        public Ress.Resource ResourceGetter
        {
            get { return localizedResources; }
        }
    }    
}
