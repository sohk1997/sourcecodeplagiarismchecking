using System;
using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebClientCore.Models.DAOs
{
    public class SubmissionDAO
    {
		private string token;

		public SubmissionDAO(string token)
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
