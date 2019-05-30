using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Customer {
    public class CustomerUpdate {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}