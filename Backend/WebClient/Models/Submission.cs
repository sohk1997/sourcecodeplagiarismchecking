using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClient.Models
{
    public class Submission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
    }
}