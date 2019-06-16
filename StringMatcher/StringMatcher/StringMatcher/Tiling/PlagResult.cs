using System;
using System.Collections.Generic;
using System.Text;

namespace StringMatcher.Tiling
{
    public class PlagResult
    {
        public List<MatchVals> tiles;
        public float similarity;
        public int id1;
        public int id2;
        public int id1StringLength;
        public int id2StringLength;
        public string algName;
        public string normName;
        public bool suspectedPlagiarism;

        public PlagResult(int id1, int id2)
        {
            this.tiles = new List<MatchVals>();
            this.similarity = (float)0.0;
            this.id1 = id1;
            this.id2 = id2;
            this.id1StringLength = id1.ToString().Length;
            this.id2StringLength = id2.ToString().Length;
            this.algName = "";
            this.normName = "";
            this.suspectedPlagiarism = false;
        }

        public PlagResult(List<MatchVals> tiles, float similarity, int id1, int id2, int id1StringLength, int id2StringLength, string algName, string normName, bool suspectedPlagiarism)
        {
            this.tiles = tiles;
            this.similarity = similarity;
            this.id1 = id1;
            this.id2 = id2;
            this.id1StringLength = id1StringLength;
            this.id2StringLength = id2StringLength;
            this.algName = algName;
            this.normName = normName;
            this.suspectedPlagiarism = suspectedPlagiarism;
        }

        public void SetTiles(List<MatchVals> tiles)
        {
            if (tiles.GetType() != new List<MatchVals>().GetType())
                Console.WriteLine("NoValidArgumentError: tiles must be of type list");

            else
                this.tiles = tiles;
        }

        public List<MatchVals> GetTiles()
        {
            return this.tiles;
        }

        public void SetSimilarity(float similarity)
        {
            if (!(0 <= similarity) && (similarity <= 1))
                Console.WriteLine("OutOfRangeError: Similarity value should be out of range 0 to 1.0");
            else
                this.similarity = similarity;
        }

        public float GetSimilarity()
        {
            return this.similarity;
        }

        public void SetIdentifier(int i, int j)
        {
            this.id1 = i;
            this.id2 = j;
        }

        public Identifiers GetIdentifier()
        {
            if (this.id1 == 0 || this.id2 == 0)
                Console.WriteLine("NoIdentifierSetError: One or both identifier were not set.");
            return (new Identifiers(this.id1, this.id2));
        }

        public bool ContainsIdentifier(String id)
        {
            return (id == this.id1.ToString() || id == this.id2.ToString());
        }

        public void SetIdStringLength(int id1StringLength, int id2StringLength)
        {
            this.id1StringLength = id1StringLength;
            this.id2StringLength = id2StringLength;
        }

        public StringLengths GetIdStringLength()
        {
            return (new StringLengths(this.id1StringLength, this.id2StringLength));
        }

        public void SetSuspectedPlagiarism(bool value)
        {
            this.suspectedPlagiarism = value;
        }

        public bool IsSuspectPlagiarism()
        {
            return this.suspectedPlagiarism;
        }

        public void SetAlgorithmName(String algName)
        {
            this.algName = algName;
        }

        public string GetAlgorithmName()
        {
            return this.algName;
        }

        public void SetNormalizerName(String normName)
        {
            this.normName = normName;
        }

        public string GetNormalizerName()
        {
            return this.normName;
        }

        public bool __eq__(PlagResult other)
        {
            if (other == null)
                return false;
            else if ((this.GetIdentifier() == other.GetIdentifier()) && (this.GetSimilarity() == other.GetSimilarity()) && (this.GetTiles() == other.GetTiles()) && (this.GetIdStringLength() == other.GetIdStringLength()))
                return true;
            return false;
        }

        public bool __ne__(PlagResult other)
        {
            return (!this.__eq__(other));
        }

        public string __str__()
        {
            string val = "PlagResult:\n"
                    + " Identifier: " + this.GetIdentifier().ToString() + '\n'
                    + " Similarity: " + this.GetSimilarity() + '\n'
                    + " Tiles: " + this.GetTiles() + "\n"
                    + " supected Plagiarism: " + this.IsSuspectPlagiarism() + '\n';
            return val;
        }

        public string __repr__()
        {
            return this.GetIdentifier().ToString() + " " + this.GetSimilarity() + " " +
                    this.GetTiles() + " " + this.IsSuspectPlagiarism();
        }
    }
}
