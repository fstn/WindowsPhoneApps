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
    public delegate void CompletedEventHandler(object sender, EventArgs e);
    public delegate void CaptureEventHandler(object sender, object e);
    public delegate void InitializedEventHandler(object sender, EventArgs e);
    public delegate void DeactivatedEventHandler(object sender, EventArgs e);
    public delegate void ActivatedEventHandler(object sender, EventArgs e);
    public delegate void ClosedEventHandler(object sender, EventArgs e);
    public delegate void NavigatedEventHandler(object sender, EventArgs e);
    
}
