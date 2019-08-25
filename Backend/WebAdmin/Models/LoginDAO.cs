using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace WebAdmin.Models
{
    public class LoginDAO
    {
        public LoginResultViewModel Login(LoginViewModel model)
        {
            var client = RequestHelper.GetHttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");;
            try
            {
                System.Console.WriteLine("content " + content.ReadAsStringAsync().Result);
                var data =  client.PostAsync("token", content).Result;
                var responseString = data.Content.ReadAsStringAsync().Result;
                var statusCode = data.StatusCode;
                if(statusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<LoginResultViewModel>(responseString);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Error");
                System.Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}