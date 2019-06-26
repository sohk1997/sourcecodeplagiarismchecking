using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace WebClient.Infrastructures
{
    public static class UrlHelperExtension
    {
        private static readonly string assetsPath = WebConfigurationManager.AppSettings["AssetsPath"].ToString();

        public static string Assets(this UrlHelper urlHelper, string path)
        {
            return urlHelper.Content($"{assetsPath}{path}");
        }
    }
}