﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebCheck
{
    public class Submission
    {
        public Guid Id { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string FileUrl { get; set; }
        public SourceCodeStatus Status { get; set; }
        public SourceCodeType Type { get; set; }
        public DateTime? UploadDate { get; set; }
        public CheckType CheckType { get; set; }
    }
}
