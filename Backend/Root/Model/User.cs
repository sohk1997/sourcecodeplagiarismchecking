using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Root.Model {
    public class User {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public int RoleId { get; set; }
    }
}