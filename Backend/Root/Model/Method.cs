using System;
namespace Root.Model
{
    public class Method
    {
        public Method()
        {
        }

        public int Id { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public string Vector { get; set; }
        public string ParseTree { get; set; }
        public string MethodString { get; set; }
        public int SourceCodeId { get; set; }
        public string MethodName { get; set; }
    }
}
