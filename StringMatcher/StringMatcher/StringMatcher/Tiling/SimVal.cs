using System;
using System.Collections.Generic;
using System.Text;

namespace StringMatcher.Tiling
{
    public class SimVal
    {
        public readonly float similarity;
        public readonly bool suspPlag;
	    public SimVal(float similarity, bool suspPlag)
            {
                this.similarity = similarity;
                this.suspPlag = suspPlag;
            }
    }
}
