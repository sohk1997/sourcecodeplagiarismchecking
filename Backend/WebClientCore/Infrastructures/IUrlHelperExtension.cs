using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace WebClientCore.Infrastructures
{
    public static class IUrlHelperExtension
    {
        private static readonly string assetsPath = "~/Assets/";

        public static string Assets(this IUrlHelper urlHelper, string path)
        {
            return urlHelper.Content($"{assetsPath}{path}");
        }
    }
}