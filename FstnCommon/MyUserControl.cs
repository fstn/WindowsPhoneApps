using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FstnCommon
{
    public abstract class MyUserControl : UserControl
    {
        public abstract void Show();
        public abstract void Close();
    }
}
