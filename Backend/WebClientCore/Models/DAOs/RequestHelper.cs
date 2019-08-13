﻿using System;
using System.Net.Http;

namespace WebClientCore.Models.DAOs
{
    public class RequestHelper
    {
        //public const string API_URL = "http://localhost:6969";
        public const string API_URL = "http://35.247.189.98";

        public static HttpClient GetHttpClient()
        {
            var result = new HttpClient();

            result.BaseAddress = new Uri(API_URL);
            return result;
        }
    }
}
