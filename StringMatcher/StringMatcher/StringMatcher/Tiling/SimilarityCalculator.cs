using System;
using System.Collections.Generic;
using System.Text;

namespace StringMatcher.Tiling
{
    public class SimilarityCalculator
    {
        public static SimVal CalcSimilarity(List<String> s1List, List<String> s2List, List<MatchVals> tiles, float threshold)
        {
            float similarity = Sim(s1List, s2List, tiles);
            bool suspPlag = false;

            if (similarity >= threshold)
                suspPlag = true;

            return (new SimVal(similarity, suspPlag));
        }

        private static float Sim(List<String> s1List,
            List<String> s2List, List<MatchVals> tiles)
        {

            return ((float)(2 * Coverage(tiles)) / (float)(s1List.Count + s2List.Count));
        }


        private static int Coverage(List<MatchVals> tiles)
        {
            int accu = 0;
            foreach (MatchVals tile in tiles)
            {
                accu += tile.length;
            }
            return accu;
        }
    }
}
