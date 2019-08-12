using System.Collections.Generic;

namespace WebAdmin.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string IsActive { get; set; }
        public int RoleId { get; set; }
        public bool IsBanned { get; set; }
        public string Role { get; set; }
    }

    public class ListUserResult
    {
        public List<User> UserList { get; set; }
    }
}