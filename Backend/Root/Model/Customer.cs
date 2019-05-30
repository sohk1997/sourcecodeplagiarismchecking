using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Model {
    public class Customer {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? UpdatedDate { set; get; }
    }
}