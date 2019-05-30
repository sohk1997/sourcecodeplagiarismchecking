using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Root.Model {
    public class User : IdentityUser {
        [Required]
        public override string UserName { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string EmployeeC { get; set; }

        public string IsActive { get; set; }
        public string RoleId { get; set; }
    }
}