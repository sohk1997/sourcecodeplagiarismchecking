using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModel
{
    public class TestValidationViewModel
    {
        [Required]
        [MaxLength(5,ErrorMessage = "Content can't be longer than 5 characters")]
        public string Content { get; set; }
    }
}
