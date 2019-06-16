using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringMatcher.Tiling
{
    public class GSTHashTable
    {
        private Dictionary<long, ICollection<int>> dict;

        public GSTHashTable()
        {
            dict = new Dictionary<long, ICollection<int>>();
        }

        public void Add(long h, int obj)
        {
            ICollection<int> newlist = null;
            if(dict.ContainsKey(h))
            {
                newlist = dict[h];
                newlist.Add(obj);
                dict[h] = newlist;
            }
            else
            {
                newlist = new List<int>();
                newlist.Add(obj);
                dict.Add(h, newlist);
            }
        }

        public List<int> Get(long key)
        {
            if (dict.ContainsKey(key))
                return dict[key].ToList();
            else
                return null;
        }

        public void Clear()
        {
            dict = new Dictionary<long, ICollection<int>>();
        }
    }
}
