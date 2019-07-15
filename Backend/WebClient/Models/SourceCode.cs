using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClient.Models
{
    public class SourceCode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string Description { get; set; }
    }
}