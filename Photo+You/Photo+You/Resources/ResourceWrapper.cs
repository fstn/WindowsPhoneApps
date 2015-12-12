using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo_You.Resources
{
    public class ResourceWrapper
    {
        private static Msg localizedResources = new Msg();

        public static  Msg ResourceGetter
        {
            get { return localizedResources; }
        }
    }    
}

