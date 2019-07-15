using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClient.ViewModels
{
    public class SourceCodeModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }

        [AllowHtml]
        public string Code { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string Description { get; set; }
    }
}