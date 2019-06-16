using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Root.Model
{
    public class Document
    {
        public Guid Id { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public byte[] DocumentContent { get; set; }
        public string DocumentExtn { get; set; }
        public long Length { get; set; }
    }
}
