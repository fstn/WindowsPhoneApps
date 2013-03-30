using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using FstnCommon.Market.Model;
using FstnCommon.Util.Settings;

namespace GiveMeSomething.Util
{
    public class URIModel
    {
        private String rootUri = "http://marketplaceedgeservice.windowsphone.com";
        private String fstnAppsUri = "http://87.98.221.216:8080/AppsWS-web/rest/apps/";
        private String fstnListOfCatsUri = "http://87.98.221.216:8080/AppsWS-web/rest/cats/";
        private String imageUri = "http://cdn.marketplaceimages.windowsphone.com/v3.2/en-US/image/";
        private String fstnRandomPartOfUriWithCat = "randomWithCat/";
        private String fstnRandomPartOfUri = "random/";
        private String randomBestPartOfUri = "randomBest/";
        private String versionUri = "v3.2";
        private String apps = "apps/";

        private URIModel()
        {

        }
        public static URIModel Instance { get; set; }
        static URIModel()
        {
            Instance = new URIModel();
        }
        public String getFstnURL()
        {
            return fstnAppsUri;
        }
        public Uri getImageUri()
        {
            return new Uri(imageUri);
        }
        public String getImageUriString()
        {
            return imageUri;
        }
        public Uri getRootUri()
        {
            return new Uri(rootUri + "/");
        }
        public Uri getBaseUri()
        {
            return new Uri(getRootUri() + versionUri + "/" + LangUri + "/");
        }
        public Uri getBaseAppsUri()
        {
            return new Uri(getRootUri() + versionUri + "/" + LangUri + "/" + apps);
        }
        public Uri getMarketCatsUri()
        {
            return new Uri(getBaseUri() + "appCategories/");
        }

        internal Uri getRandomWithCat(MarketCat categorie)
        {
            return getRandomWithCat(categorie.Id);
        }

        internal Uri getRandomWithCat(string categoryId)
        {
            Double minRateValue = SettingsService.Instance.Value<Double>("minRateValue");
            int count = SettingsService.Instance.Value<int>("minCount"); ;
            String rand = new Random().Next().ToString();
            return new Uri(fstnAppsUri + fstnRandomPartOfUriWithCat + categoryId + "/" + minRateValue + "/" + count + "/?" + rand);
        }
        public Uri getFstnListOfCatsUri()
        {
            return new Uri(fstnListOfCatsUri);
        }
        public Uri getFstnRandomBestApp()
        {
            return new Uri(fstnAppsUri + randomBestPartOfUri + "?r=" + DateTime.Now.ToString());
        }

        public string LangUri
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

    }
}
