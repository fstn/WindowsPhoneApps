using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnCommon.Util
{
    public class MathUtil
    {
        public static  float Lerp( float start,  float stop,  float amt) {
		    return start + (stop - start) * amt;
	    }

	    public static  float Norm( float value,  float start,  float stop) {
		    return (value - start) / (stop - start);
	    }

    }
}
