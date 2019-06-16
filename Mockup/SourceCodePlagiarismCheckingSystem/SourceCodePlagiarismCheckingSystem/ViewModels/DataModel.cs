using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.ViewModels
{
    public class DataRecordModel
    {
        public int Id { get; set; }

        [Display(Name = "Drink Name")]
        public string Drink { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}