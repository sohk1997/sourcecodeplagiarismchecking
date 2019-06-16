using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.ViewModels
{
    public class CountryRecordModel
    {
        public Guid Id { get; set; }
        public string ISO3 { get; set; }
        public string CountryName { get; set; }
    }
}