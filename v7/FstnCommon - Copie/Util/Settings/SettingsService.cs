using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace FstnCommon.Util.Settings
{
    public class SettingsService
    {
        private static SettingsService instance =new SettingsService();
        public static SettingsService Instance {get{return instance;}}
        private IsolatedStorageSettings settings;
        private SettingsService ()
	    {
             settings = IsolatedStorageSettings.ApplicationSettings;

	    }

        public T Value<T> (String key){
            T value ;
            settings.TryGetValue<T>(key, out value);
            return value;
        }

        public void Save(String key, Object value)
        {
            if (settings.Contains(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
            settings.Save();
        }

           
    }
}
