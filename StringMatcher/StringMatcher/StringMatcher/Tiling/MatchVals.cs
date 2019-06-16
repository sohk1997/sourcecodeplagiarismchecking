using System;
using System.Collections.Generic;
using System.Text;

namespace StringMatcher.Tiling
{
    public class MatchVals
    {
        public readonly int patternPostion;
        public readonly int textPosition;
        public readonly int length;
        public MatchVals(int p, int t, int l)
        {
            this.patternPostion = p;
            this.textPosition = t;
            this.length = l;
        }
    }
}
