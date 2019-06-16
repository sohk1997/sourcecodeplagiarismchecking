using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string ISO3 { get; set; }
        public string CountryName { get; set; }

        //Relationship
        public IList<User> Users { get; set; }
    }
}