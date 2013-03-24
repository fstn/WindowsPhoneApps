using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FstnUserControl.Error
{
    public interface IErrorDisplayer
    {
        void Show(String text);
    }
}
