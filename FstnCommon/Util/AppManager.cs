using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FstnCommon.Util
{
    public class AppManager
    {
        public event DeactivatedEventHandler Deactivated;
        public event ActivatedEventHandler Activated;
        public event ClosedEventHandler Closed;
        public event NavigatedEventHandler Navigated;
        

        #region Singleton

        private static AppManager _instance = null;

        private AppManager() { }

        public static AppManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    AppManager instance = new AppManager();
                    instance.Initialize();
                    _instance = instance;
                }
                return _instance;
            }
        }

        #endregion

        private void Initialize()
        {

        }

        
	

        public void Deactivate()
        {
            if (Deactivated != null)
                Deactivated(this, null);
        }
        public void Activate()
        {
            if (Activated != null)
                Activated(this, null);
        }
        public void Close()
        {
            if (Closed != null)
                Closed(this, null);
        }

        public void Navigate()
        {
            if (Navigated != null)
                Navigated(this, null);
        }
    }
}
