using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Customer {
    public class CustomerCreate {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}