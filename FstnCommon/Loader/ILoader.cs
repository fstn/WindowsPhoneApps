using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnCommon.Loader
{
    public interface ILoader
    {
        void load(Uri uri);
    }
}
