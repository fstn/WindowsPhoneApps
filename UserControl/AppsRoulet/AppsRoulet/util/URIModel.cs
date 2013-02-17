using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsRoulet.util
{
    public class URIModel
    {
        private String rootUri = "http://marketplaceedgeservice.windowsphone.com";
        private String versionUri = "v3.2";
        private String langUri = "fr-FR";
        private static URIModel instance=new URIModel();

        private URIModel()
        {

        }
        public static URIModel getInstance()
        {
            return instance;
        }

        public Uri getRootUri()
        {
            return new Uri(rootUri+"/");
        }
        public Uri getBaseUri()
        {
            return new Uri(getRootUri()+versionUri+"/"+langUri+"/");
        }
        public Uri getMarketCatsUri()
        {
            return new Uri(getBaseUri()+"appCategories/");
        }
    }
}
