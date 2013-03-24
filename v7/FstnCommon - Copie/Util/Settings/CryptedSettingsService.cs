using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FstnCommon.Util.Settings
{
    public class CryptedSettingsService
    {
        private static CryptedSettingsService instance = new CryptedSettingsService();
        public static CryptedSettingsService Instance { get { return instance; } }
        private IsolatedStorageSettings settings;
        private CryptedSettingsService()
	    {
             settings = IsolatedStorageSettings.ApplicationSettings;

	    }

        public String Value (String key){
            byte[] cryptValue;
            settings.TryGetValue<byte[]>(key, out cryptValue);
            byte[] decryptValue = ProtectedData.Unprotect(cryptValue, null);
            String value = UTF8Encoding.UTF8.GetString(decryptValue, 0, decryptValue.Length); 
            return value;
        }

        public void Save(String key, Object value)
        {
            var cryptValue = ProtectedData.Protect(UTF8Encoding.UTF8.GetBytes(value.ToString()), null);
            if (settings.Contains(key))
            {
                settings[key] = cryptValue;
            }
            else
            {
                settings.Add(key, cryptValue);
            }

        }

           
    }
}
