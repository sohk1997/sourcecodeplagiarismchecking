using StringMatcher.Tiling;
using System;
using System.Text.RegularExpressions;

namespace StringMatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            GreedyStringTiling.Run("int b  = 1;int c = 5;int a  = 0;", "int a = 0;int b = 1;", 2, 0.1f);
        }
    }
}
