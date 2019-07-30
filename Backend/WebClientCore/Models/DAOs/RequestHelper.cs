using System;
using System.Net.Http;

namespace WebClient.Models.DAOs
{
    public class RequestHelper
    {
        private const string API_URL = "http://35.247.189.98";

        public static HttpClient GetHttpClient()
        {
            var result = new HttpClient();

            result.BaseAddress = new Uri(API_URL);
            return result;
        }
    }
}
