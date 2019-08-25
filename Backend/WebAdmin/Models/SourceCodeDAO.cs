using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class SourceCodeDAO
    {
        private string token;

        public SourceCodeDAO(string token)
        {
            this.token = token;
        }
        public async Task<List<DocumentInList>> GetAllSubmission()
        {
            var client = RequestHelper.GetHttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            using (var response = await client.GetAsync("api/document/"))
            {
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<DocumentInList>>(body);
                return result;
            }
        }
    }
}
