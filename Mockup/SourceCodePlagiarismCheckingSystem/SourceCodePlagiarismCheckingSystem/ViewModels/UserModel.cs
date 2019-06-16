using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.ViewModels
{
    public class UserRecordModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }

        [Display(Name = "Birthday")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DayOfBirth { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        public Guid CountryId { get; set; }
        public Boolean isActive { get; set; }
        public string CountryName { get; set; }
        public string CountryISO3 { get; set; }
    }
}