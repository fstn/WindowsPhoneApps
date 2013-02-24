using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnCommon.Loader
{
    public delegate void LoaderLoadedEventHandler(object sender, Object obj);
    public delegate void LoaderErrorEventHandler(object sender, Object obj);
    public interface ILoader
    {
         void load(Uri uri);
    }
}
