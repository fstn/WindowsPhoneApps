using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Controls;

namespace FstnCommon
{
    public class Session
    {
        private static Session instance;
        private Dictionary<String, List<MyUserControl>> listOfUserControl;

        public Dictionary<String, List<MyUserControl>> ListOfUserControl
        {
            get { return listOfUserControl; }
            set { listOfUserControl = value; }
        }

        private Session()
        {
            listOfUserControl = new Dictionary<String, List<MyUserControl>>();
        }

        public void addUserControl(String page,MyUserControl userControl)
        {
            if (!listOfUserControl.Keys.Contains(page))
            {
                List<MyUserControl> list = new List<MyUserControl>();
                listOfUserControl.Add(page, list);
            }
            listOfUserControl[page].Add(userControl);
        }

        public void clearUserControl(String page)
        {
            listOfUserControl.Remove(page);
        }


        public static Session Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Session();
                }
                return instance;
            }
        }
    }
}