using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using InTheHand.Windows.Controls;

namespace WBEX.SWE.Performance
{
    class Program
    {
        static void Main()
        {
            WBEX.SWE.Performance.App application = new WBEX.SWE.Performance.App();

            //register custom controls
            application.RegisterControl(typeof(WBEX.SWE.Performance.MainPage));


            WBEX.SWE.Performance.MainPage mp = new WBEX.SWE.Performance.MainPage();
            application.RootVisual = mp;
            application.Run();
        }
    }
}