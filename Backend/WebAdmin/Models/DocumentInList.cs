﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class DocumentInList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string UploadDate { get; set; }
    }
}