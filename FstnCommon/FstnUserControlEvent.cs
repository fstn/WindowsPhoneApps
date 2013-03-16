using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace FstnCommon
{
    public delegate void LoaderErrorEventHandler(object sender, Object obj);
    public delegate void LoaderLoadedEventHandler(object sender, Object obj);
    public delegate void BackgroundIsMaskAnimationEventHandler();
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public delegate void CaptureEventHandler(object sender, BitmapImage e);
    public delegate void InitializedEventHandler(object sender, EventArgs e);
}
