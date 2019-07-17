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

    public class DocumentResult
    {
        public List<DocumentResultDetail> Details { get; set; }
        public float GeneralSimRatio { get; set; }
        public string FileName { get; set; }
    }

    public class DocumentResultDetail
    {
        public string MethodName { get; set; }
        public string BaseMethod { get; set; }
        public string SimMethod { get; set; }
        public SimilarityPositions Position { get; set;}
        public float SimRatio { get; set; }
    }

    public class SimilarityPositions
    {
        public List<PositionDetail> SourcePositions { get; set; }
        public List<PositionDetail> SimPositions { get; set; }

    }

    public class PositionDetail
    {
        public int StartLine { get; set; }
        public int EndLine { get; set; }
    }
}
