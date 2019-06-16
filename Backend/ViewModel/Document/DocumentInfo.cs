using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.Document
{
    public class DocumentInfo
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Extn { get; set; }
    }
}
