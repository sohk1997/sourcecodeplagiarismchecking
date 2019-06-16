using System;
using System.Collections.Generic;
using System.Text;

namespace StringMatcher.Tiling
{
    public class StringLengths
    {
        public readonly int id1StringLength;
        public readonly int id2StringLength;
        public StringLengths(int id1StringLength, int id2StringLength)
        {
            this.id1StringLength = id1StringLength;
            this.id2StringLength = id2StringLength;
        }
    }
}
