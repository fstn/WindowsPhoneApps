using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using FstnCommon;

namespace FstnAnimation
{
    public class AnimationFactory
    {
        private static AnimationFactory instance;
        private DispatcherTimer dt;
        private Queue<MyUserControl> tmpQueue;
        public static AnimationFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AnimationFactory();
                }
                return instance;
            }
        }
        private AnimationFactory()
        {
            tmpQueue = new Queue<MyUserControl>();
        }
        public void Open(String page)
        {
            try
            {
                foreach (MyUserControl userControl in Session.Instance.ListOfUserControl[page])
                {
                    tmpQueue.Enqueue(userControl);
                }
            }
            catch (KeyNotFoundException kfe)
            {
                Debugger.Log(3, "initial load", "can't load key in session "+page);
            }
            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 150); 
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }
        public void dt_Tick(object sender, EventArgs e)
        {
            if (tmpQueue.Count > 0)
            {
                MyUserControl userControl = tmpQueue.Dequeue();
                userControl.Show();
            }
            else
            {
                dt.Stop();
            }
        }
        public void Close(String page)
        {
            try
            {
                foreach (MyUserControl userControl in Session.Instance.ListOfUserControl["MainPage"])
                {
                    userControl.Close();

                }
            }
            catch (KeyNotFoundException kfe)
            {
                Debugger.Log(3, "initial load", "can't load key in session");
            }
        }
    }
}