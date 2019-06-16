using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StringMatcher.Tiling
{
    public class GreedyStringTiling
    {
        public static List<MatchVals> tiles = new List<MatchVals>();
        public static List<Queue<MatchVals>> matchList = new List<Queue<MatchVals>>();

        /**
	 * This method runs a comparison on the two given strings s1 and s2
	 * returning a PlagResult object containing the similarity value, the
	 * similarities as list of tiles and a bool value indicating suspected
	 * plagiarism.
	 * 
	 * Input: s1 and s2 : normalized Strings mML : minimumMatchingLength
	 * threshold : a single value between 0 and 1 Output: PlagResult
	 * 
	 * @param s1
	 * @param s2
	 * @param mML
	 * @param threshold
	 * @return 
	 */
        public static PlagResult Run(String s1, String s2, int mML, float threshold)
        {
            if (mML < 1)
                Console.WriteLine("OutOfRangeError: minimum Matching Length mML needs to be greater than 0");
            if (!((0 <= threshold) && (threshold <= 1)))
                Console.WriteLine("OutOfRangeError: treshold t needs to be 0<=t<=1");
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                Console.WriteLine("NoValidArgumentError: input must be of type string not None");

            // Compute Tiles
            tiles = RKR_GST(s1, s2, mML, 20);

            // Compute Similarity
            Regex splitRegex = new Regex(@"\s+|\W+");

            SimVal simResult = SimilarityCalculator.CalcSimilarity(
                    new List<string>(splitRegex.Split(s1)), new List<string>(splitRegex.Split(s2)),
                    tiles, threshold);
            float similarity = simResult.similarity;
            if (similarity > 1)
                similarity = 1;

            // Create Plagiarism result and set attributes
            PlagResult result = new PlagResult(0, 0);
            result.SetIdentifier(CreateKRHashValue(s1), CreateKRHashValue(s2));
            result.SetTiles(tiles);
            result.SetSimilarity(similarity);
            result.SetSuspectedPlagiarism(simResult.suspPlag);


            Console.WriteLine("Identifiers: " + result.GetIdentifier().id1 + ":" + result.GetIdentifier().id2);
            Console.WriteLine("Similarity: " + result.GetSimilarity());
            Console.WriteLine("Plagiriasm tiles: ");

            string[] s1arr = splitRegex.Split(s1);
            string[] s2arr = splitRegex.Split(s2);

            foreach (MatchVals tiles in result.GetTiles())
            {
                Console.Write("(" + tiles.patternPostion + ",");
                Console.Write(tiles.textPosition + ",");
                Console.WriteLine(tiles.length + ")");

                for (int i = 0; i < tiles.length; i++)
                {
                    Console.Write(s1arr[tiles.patternPostion + i] + " ");
                }
                Console.WriteLine("");
                for (int i = 0; i < tiles.length; i++)
                {
                    Console.Write(s2arr[tiles.textPosition + i] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\nSuspected Plagirism: " + result.suspectedPlagiarism);

            return result;
        }

        /**
         * Computes Running-Karp-Rabin-Greedy-String-Tiling.
         * 
         * P pattern string T text string
         * 
         * More Informations can be found here: "String Similarity via Greedy String
         * Tiling and Running Karp-Rabin Matching"
         * http://www.pam1.bcs.uwa.edu.au/~michaelw/ftp/doc/RKR_GST.ps "YAP3:
         * Improved Detection of Similarities in Computer Program and other Texts"
         * http://www.pam1.bcs.uwa.edu.au/~michaelw/ftp/doc/yap3.ps
         * 
         * @author arunjayapal
         * @param Pattern
         *            string
         * @param Text
         *            String
         * @param minimal
         *            Matching Length value
         * @param Initialize
         *            search size
         * @return tiles
         */
        public static List<MatchVals> RKR_GST(String P, String T,
                int minimalMatchingLength, int initsearchSize)
        {
            if (minimalMatchingLength < 1)
                minimalMatchingLength = 3;

            if (initsearchSize < 5)
                initsearchSize = 20;
            Regex splitRegex = new Regex(@"\s+|\W+");
            String[] PList = splitRegex.Split(P);
            String[] TList = splitRegex.Split(T);

            int s = initsearchSize;
            bool stop = false;

            while (!stop)
            {
                // Lmax is size of largest maximal-matches from this scan
                int Lmax = ScanPattern(s, PList, TList);
                // if very long string no tiles marked. Iterate with larger s
                if (Lmax > 2 * s)
                    s = Lmax;
                else
                {
                    MarkStrings(s, PList, TList);
                    if (s > (2 * minimalMatchingLength))
                        s = s / 2;
                    else if (s > minimalMatchingLength)
                        s = minimalMatchingLength;
                    else
                        stop = true;
                }
            }
            return tiles;
        }

        /**
         * Scans the pattern and text string lists for matches.
         * 
         * If a match is found that is twice as big as the search Length s that size
         * is returned, to be used to restart the scanpattern with it. All matches
         * found are stored in a list of matches in queues.
         * 
         * @param s
         * @param P
         * @param T
         * @return Longest maximum match
         */
        public static int ScanPattern(int s, String[] P, String[] T)
        {

            int longestMaxMatch = 0;
            Queue<MatchVals> queue = new Queue<MatchVals>();
            GSTHashTable hashtable = new GSTHashTable();
            /**
             * Starting at the first unmarked token in T for each unmarked Tt do if
             * distance to next tile <= s then advance t to first unmarked token
             * after next tile else create the KR-hash value for substring Tt to
             * Tt+s-1 and add to hashtable
             */
            int t = 0;
            bool noNextTile = false;
            int h;
            while (t < T.Length)
            {
                if (IsMarked(T[t]))
                {
                    t = t + 1;
                    continue;
                }

                int dist;
                if (DistToNextTile(t, T).HasValue)
                    dist = (int)DistToNextTile(t, T);

                else
                {
                    dist = 0;
                    dist = T.Length - t;
                    noNextTile = true;
                }
                //int dist = distToNextTile(t, T);
                // No next tile found

                if (dist < s)
                {
                    if (noNextTile)
                        t = T.Length;
                    else
                    {
                        if (JumpToNextUnmarkedTokenAfterTile(t, T).HasValue)
                            t = (int)JumpToNextUnmarkedTokenAfterTile(t, T);

                        else
                            t = T.Length;
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = t; i <= t + s - 1; i++)
                        sb.Append(T[i]);
                    String substring = sb.ToString();
                    h = CreateKRHashValue(substring);
                    hashtable.Add(h, t);
                    t = t + 1;
                }
            }

            /**
             * Starting at the first unmarked token of P for each unmarked Pp do if
             * distance to next tile <= s then advance p to first unmarked token
             * after next tile else create the KR hash-value for substring Pp to
             * Pp+s-1 check hashtable for hash of KR hash-value for each hash-table
             * entry with equal hashed KR hash-value do if for all j from 0 to s-1,
             * Pp+ j = Tt+ j then k: = s while Pp+k = Tt+k AND unmarked(Pp+k) AND
             * unmarked(Tt+k) do k := k + 1 if k > 2 *s then return(k) else record
             * new maximal-match
             */
            noNextTile = false;
            int p = 0;
            while (p < P.Length)
            {
                if (IsMarked(P[p]))
                {
                    p = p + 1;
                    continue;
                }

                int dist;

                if (DistToNextTile(p, P).HasValue)
                {
                    dist = (int)DistToNextTile(p, P);
                }

                else
                {
                    dist = 0;
                    dist = P.Length - p;
                    noNextTile = true;
                }

                if (dist < s)
                {
                    if (noNextTile)
                        p = P.Length;
                    else
                    {

                        if (JumpToNextUnmarkedTokenAfterTile(p, P).HasValue)
                            p = (int)JumpToNextUnmarkedTokenAfterTile(p, P);

                        else
                        {
                            p = 0;
                            p = P.Length;
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = p; i <= p + s - 1; i++)
                    {
                        sb.Append(P[i]);
                    }
                    String substring = sb.ToString();
                    h = CreateKRHashValue(substring);
                    List<int> values = hashtable.Get(h);
                    if (values != null)
                    {
                        foreach (int val in values)
                        {
                            StringBuilder newsb = new StringBuilder();
                            for (int i = val; i <= val + s - 1; i++)
                            {
                                newsb.Append(T[i]);
                            }
                            if (newsb.ToString() == substring)
                            {
                                t = val;
                                int k = s;

                                while (p + k < P.Length && t + k < T.Length
                                        && P[p + k] == T[t + k]
                                        && IsUnmarked(P[p + k])
                                        && IsUnmarked(T[t + k]))
                                    k = k + 1;

                                if (k > 2 * s)
                                    return k;
                                else
                                {
                                    if (longestMaxMatch < s)
                                        longestMaxMatch = s;
                                    MatchVals mv = new MatchVals(p, t, k);
                                    queue.Enqueue(mv);
                                }
                            }
                        }
                    }
                    p += 1;
                }

            }
            if (queue.Count > 0)
            {
                matchList.Add(queue);
            }
            return longestMaxMatch;
        }

        private static void MarkStrings(int s, String[] P, String[] T)
        {
            foreach (Queue<MatchVals> queue in matchList)
            {
                while (queue.Count > 0)
                {
                    MatchVals match = queue.Dequeue();
                    if (!isOccluded(match, tiles))
                    {
                        for (int j = 0; j < match.length; j++)
                        {
                            P[match.patternPostion + j] = MarkToken(P[match.patternPostion + j]);
                            T[match.textPosition + j] = MarkToken(T[match.textPosition + j]);
                        }
                        tiles.Add(match);
                    }
                }
            }
            matchList = new List<Queue<MatchVals>>();
        }

        /**
         * Creates a Karp-Rabin Hash Value for the given substring and returns it.
         * 
         * Based on: http://www-igm.univ-mlv.fr/~lecroq/string/node5.html
         * 
         * @param substring
         * @return hash value for any given string
         */

        private static int CreateKRHashValue(String substring)
        {
            int hashValue = 0;
            for (int i = 0; i < substring.Length; i++)
                hashValue = ((hashValue << 1) + (int)substring[i]);
            return hashValue;
        }

        /**
         * If string s is unmarked returns True otherwise False.
         * 
         * @param string
         * @return true or false (i.e., whether marked or unmarked)
         */
        private static bool IsUnmarked(String s)
        {
            if (s.Length > 0 && s[0] != '*')
                return true;
            else
                return false;
        }

        private static bool IsMarked(String s)
        {
            return (!IsUnmarked(s));
        }

        private static String MarkToken(String s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("*");
            sb.Append(s);
            return sb.ToString();
        }

        /**
         * Returns true if the match is already occluded by another match in the
         * tiles list.
         * 
         * "Note that "not occluded" is taken to mean that none of the tokens Pp to
         * Pp+maxmatch-1 and Tt to Tt+maxmatch-1 has been marked during the creation
         * of an earlier tile. However, given that smaller tiles cannot be created
         * before larger ones, it suffices that only the ends of each new putative
         * tile be testet for occlusion, rather than the whole maxmimal match." [
         * "String Similarity via Greedy String Tiling and Running Karp-Rabin Matching"
         * http://www.pam1.bcs.uwa.edu.au/~michaelw/ftp/doc/RKR_GST.ps]
         * 
         * @param match
         * @param tiles2
         * @return true or false
         */
        private static bool isOccluded(MatchVals match, List<MatchVals> tiles)
        {
            if (tiles == null || tiles == null || tiles.Count == 0)
                return false;
            foreach (MatchVals matches in tiles)
            {
                if ((matches.patternPostion + matches.length == match.patternPostion
                        + match.length)
                        && (matches.textPosition + matches.length == match.textPosition
                        + match.length))
                    return true;
            }
            return false;
        }

        /**
         * Returns distance to next tile, i.e. to next marked token. If not tile was
         * found, it returns None.
         * 
         * case 1: there is a next tile -> pos + dist = first marked token -> return
         * dist case 2: there is no next tile -> pos + dist = len(stringList) ->
         * return None dist is also number of unmarked token 'til next tile
         * 
         * @param p
         * @param p2
         * @return distance to next tile
         */
        private static int? DistToNextTile(int pos, String[] stringList)
        {
            if (pos == stringList.Length)
                return null;
            int dist = 0;
            while (pos + dist + 1 < stringList.Length && IsUnmarked(stringList[pos + dist + 1]))
                dist += 1;
            if (pos + dist + 1 == stringList.Length)
                return null;
            return dist + 1;
        }

        /**
         * Returns the first postion of an unmarked token after the next tile.
            case 1: -> normal case
                -> tile exists
                -> there is an unmarked token after the tile
            case 2:
                -> tile exists
                -> but NO unmarked token after the tile
            case 3:
                -> NO tile exists
         * @param pos
         * @param stringList
         * @return the position to jump to the next unmarked token after tile
         */
        private static int? JumpToNextUnmarkedTokenAfterTile(int pos, String[] stringList)
        {
            var dist = DistToNextTile(pos, stringList);
            if (dist.HasValue)
                pos = pos + (int)dist;

            else
                return null;
            while (pos + 1 < stringList.Length && (IsMarked(stringList[pos + 1])))
                pos = pos + 1;
            if (pos + 1 > stringList.Length - 1)
                return null;
            return pos + 1;
        }
    }
}
