using System;
using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Models.DAOs;
using Newtonsoft.Json;

namespace WebClientCore.Models.DAOs
{
    public class SubmissionDAO
    {
        public async Task<List<DocumentInList>> GetAllSubmission()
        {
            var client = RequestHelper.GetHttpClient();
            using (var response = await client.GetAsync("api/document/"))
            {
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<DocumentInList>>(body);
                return result;
            }
        }
    }
}
