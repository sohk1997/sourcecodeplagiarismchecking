using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCheck
{
    public class Block
    {
        public String Content { get; set; }
        public int OpenBracket { get; set; }
        public int CloseBracket { get; set; }
        public Block(int openBracket, int closeBracket)
        {
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
        }
        public Block()
        {
        }
        public Block(int openBracket)
        {
            OpenBracket = openBracket;
        }
    }
}
