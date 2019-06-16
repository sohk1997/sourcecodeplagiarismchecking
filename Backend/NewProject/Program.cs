using Newtonsoft.Json;
using StringMatcher.Tiling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace GetMethod
{

    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";
            //string path = @"E:\Study\FPTU\8thSemester\PRX301\CPS\git\CPS\source\WebApp\LaptopSuggestion\src\java\emvh\crawler\BenmarkCrawler.java";
            string path = @"E:\Study\FPTU\8thSemester\PRX301\CPS\git\CPS\source\WebApp\LaptopSuggestion\src\java\emvh\dao\CpuDAO.java";
            //string path = @"E:\Study\FPTU\9thSemester\SWP49X\Example\GetMethod\GetMethod\Block.cs";
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            string str = "";
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    str = Regex.Replace(str,
                               blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                               me =>
                               {
                                   if (me.Value.StartsWith("") || me.Value.StartsWith("//"))
                                       return me.Value.StartsWith("//") ? Environment.NewLine : "";
                                   // Keep the literal strings
                                   return me.Value;
                               }, RegexOptions.Singleline);
                    str += string.Join(" ", line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    //str += line;
                }
            }
            List<Block> blocks = GetBlocks(str);
            List<Model.Item> results = new List<Model.Item>();
            int i = 0;
            int time = 0;
            foreach (var block in blocks)
            {
                time++;
                if (time == 5)
                {
                    break;
                }
                Console.WriteLine("==============================");
                Console.WriteLine("Block: " + ++i);
                Console.WriteLine("Open at: " + block.OpenBracket);
                Console.WriteLine("Close at: " + block.CloseBracket);
                Console.WriteLine("Content: " + block.Content);
                Console.WriteLine("==============================");
                //string jsonData = GET("https://api.github.com/search/code?access_token=32bdd3b86211cbf4c0756036d2507f144696d8b1&q=" + block.Content);
                string jsonData = GET("https://api.github.com/search/code?access_token=8686354be8f60ec38e5c4138647cda6e4e8c972d&q=" + HttpUtility.UrlEncode(block.Content));
                var gitCode = JsonConvert.DeserializeObject<Model.RootObject>(jsonData);
                foreach (var item in gitCode.items)
                {
                    //Check if item is exist in result
                    if (results.Contains(item))
                    {
                        results.Find(it => it.html_url == item.html_url).appearance++;
                    }
                    else
                    {
                        results.Add(item);
                        var raw = "https://raw.githubusercontent.com/" + item.repository.full_name + "/master/" + item.path;
                        var fCode = GET(raw);
                        Console.WriteLine(block.Content);
                        Console.WriteLine("==============");
                        Console.WriteLine(fCode);
                        GreedyStringTiling.Run(block.Content, fCode, 2, 0.1f);
                    }
                }
            }
            //time = 0;
            //foreach (var result in results)
            //{
            //    if (++time == 5)
            //    {
            //        break;
            //    }
            //    Console.WriteLine("=================================");
            //    Console.WriteLine("Appearance: " + result.appearance);
            //    Console.WriteLine("=================================");
            //}
            Console.ReadKey();

        }

        // Function to find index of closing  
        // bracket for given opening bracket.  
        static int GetBracketsIndex(String expression, int index)
        {
            int i;

            // If index given is invalid and is  
            // not an opening bracket.  
            if (expression[index] != '{')
            {
                //Console.Write(expression + ", "
                //        + index + ": -1\n");
                return -1;
            }

            // Stack to store opening brackets.  
            Stack st = new Stack();

            // Traverse through string starting from  
            // given index.  
            for (i = index; i < expression.Length; i++)
            {

                // If current character is an  
                // opening bracket push it in stack.  
                if (expression[i] == '{')
                {
                    st.Push((int)expression[i]);
                } // If current character is a closing  
                  // bracket, pop from stack. If stack  
                  // is empty, then this closing  
                  // bracket is required bracket.  
                else if (expression[i] == '}')
                {
                    st.Pop();
                    if (st.Count == 0)
                    {
                        //Console.Write(expression + ", "
                        //        + index + ": " + i + "\n");
                        return i;
                    }
                }
            }

            // If no matching closing bracket  
            // is found.  
            //Console.Write(expression + ", "
            //        + index + ": -1\n");
            return -1;
        }
        public static int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        // Function to find index of closing  
        // bracket for given opening bracket.  
        static List<Block> GetBlocks(String expression)
        {
            // List to store block
            List<Block> blocks = new List<Block>();
            // Stack to store opening brackets.  
            Stack<Block> st = new Stack<Block>();

            // Traverse through string
            for (int i = 0; i < expression.Length; i++)
            {

                // If current character is an  
                // opening bracket push Block with openbracket's index into stack.  
                if (expression[i] == '{')
                {
                    st.Push(new Block(i));
                } // If current character is a closing  
                  // bracket, pop from stack, set closebracket's index and add to list
                else if (expression[i] == '}')
                {

                    Block block = st.Pop();
                    block.CloseBracket = i;
                    //Find nearly open or close bracket
                    int preBracket = 0;
                    //Reverse browsing string
                    for (int j = block.OpenBracket - 1; j > 0; j--)
                    {
                        if (expression[j] == '{' || expression[j] == '}')
                        {
                            preBracket = j;
                            break;
                        }

                    }
                    block.Content = expression.Substring(preBracket + 1, (block.CloseBracket - preBracket));
                    blocks.Add(block);

                }
            }

            return blocks;
        }

        // Returns JSON string
        static string GET(string url)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                //wc.Headers.Add("user-agent", "MyRSSReader/1.0");
                //wc.Headers["Accept"] = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, *";
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                //string _UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                //wc.Headers.Add(HttpRequestHeader.UserAgent, _UserAgent);
                json = wc.DownloadString(url);
            }
            return json;
        }
    }
}
