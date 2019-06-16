using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePicture { get; set; }
        public Guid? CountryId { get; set; }
        public Boolean isActive { get; set; }

        //Relationship
        public Country Country { get; set; }
    }
}