using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketRoulet.Util
{
    public class URIModel
    {
        private String rootUri = "http://marketplaceedgeservice.windowsphone.com";
        private String versionUri = "v3.2";
        private String langUri = "en-US";
        private String fstnAppsUri = "http://87.98.221.216:8080/AppsWS-web/rest/apps/";
        private String fstnListOfCatsUri = "http://87.98.221.216:8080/AppsWS-web/rest/cats/";
        private String randomWithCat = "randomWithCat/";
        private String fstnRandomPartOfUri= "random/";
        private String randomBest = "randomBest/";
        private String apps = "apps/";
        private String imageUri = "http://cdn.marketplaceimages.windowsphone.com/v3.2/en-US/image/";
        private static URIModel instance;

        private URIModel()
        {

        }
        public static URIModel Instance { get; set; }
        static URIModel()
        {
            Instance = new URIModel();
        }
        public String getFstnURL(){
            return fstnAppsUri;
        }
        public Uri getImageUri()
        {
            return new Uri(imageUri);
        }
        public String getImageUriString()
        {
            return  imageUri;
        }
        public Uri getRootUri()
        {
            return new Uri(rootUri+"/");
        }
        public Uri getBaseUri()
        {
            return new Uri(getRootUri()+versionUri+"/"+langUri+"/");
        }
        public Uri getBaseAppsUri()
        {
            return new Uri(getRootUri() + versionUri + "/" + langUri + "/"+apps);
        }
        public Uri getMarketCatsUri()
        {
            return new Uri(getBaseUri()+"appCategories/");
        }

        internal Uri getRandomWithCat(String categorie)
        {
            String rand = new Random().Next().ToString();
            return new Uri(fstnAppsUri + fstnRandomPartOfUri +categorie+"/?"+ rand);
        }
        public Uri getFstnListOfCatsUri()
        {
            return new Uri(fstnListOfCatsUri );
        }
        public Uri getFstnRandomBestApp()
        {
            return new Uri(fstnAppsUri+randomBest+"?r="+DateTime.Now.ToString() );
        }
    }
}
