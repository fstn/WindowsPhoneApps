using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnUserControl
{
    public delegate void LoaderErrorEventHandler(object sender, Object obj);
    public delegate void LoaderLoadedEventHandler(object sender, Object obj);
    public delegate void BackgroundIsMaskAnimationEventHandler();
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    class FstnUserControlEvent
    {
    }
}
