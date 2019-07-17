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

    public class Result
    {
        public List<ResultDetail> Details { get; set; }
        public MergeDetail MergeDetail { get; set;}
        public float GeneralSimRatio { get; set; }
        public string FileName { get; set; }
    }

    public class MergeDetail
    {
        public string BaseMethod{get;set;}
        public string SimMethod{get;set;}
        public List<PositionDetail> SourcePositions { get; set; }
        public List<PositionDetail> SimPositions { get; set; }
   
    }

    public class ResultDetail
    {
        public string BaseMethod { get; set; }
        public string SimMethod { get; set; }
        public Positions Position { get; set; }
        public float SimRatio { get; set; }
    }

    public class Positions
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