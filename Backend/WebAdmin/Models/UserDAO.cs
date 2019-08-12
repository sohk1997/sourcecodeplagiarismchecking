using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebAdmin.Models
{
    public class UserDAO
    {
        private string token;

        public UserDAO(string token)
        {
            this.token = token;
        }
        public async Task<List<User>> GetUsers()
        {
            System.Console.WriteLine("Start ");
            var client = RequestHelper.GetHttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            using (var response = await client.GetAsync("user"))
            {
                System.Console.WriteLine(response.StatusCode);
                var body = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(body);
                var result = JsonConvert.DeserializeObject<ListUserResult>(body).UserList;
                result.ForEach(u => { u.Role = u.RoleId == 1 ? "USER" : "ADMIN"; u.IsBanned = u.IsActive == "true" ? false : true; });
				return result;
            }
        }
    }
}