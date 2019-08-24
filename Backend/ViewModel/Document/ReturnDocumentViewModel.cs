using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Document
{
    public class ReturnDocumentViewModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<DocumentInList> Data { get; set; }
    }
}
