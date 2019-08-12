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
                System.Console.WriteLine("status code");
                var statusCode = data.StatusCode;
                System.Console.WriteLine(data.StatusCode);
                if(statusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                System.Console.WriteLine("Response success");
                System.Console.WriteLine(responseString);
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