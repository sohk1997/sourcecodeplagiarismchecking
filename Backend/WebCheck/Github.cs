using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;

namespace WebCheck
{
    public class Github : WebCheck
    {
        public Result Check(string content)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";
            string str = content;
            str = Regex.Replace(str,
                        blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                        me =>
                        {
                            if (me.Value.StartsWith("") || me.Value.StartsWith("//"))
                                return me.Value.StartsWith("//") ? Environment.NewLine : "";
                            // Keep the literal strings
                            return me.Value;
                        }, RegexOptions.Singleline);
            List<Block> blocks = GetBlocks(str);
            List<Item> results = new List<Item>();
            int i = 0;
            foreach (var block in blocks)
            {
                while (block.Content.Length > 0)
                {
                    var searchContent = block.Content.Substring(0, Math.Min(block.Content.Length, 100));

                    string jsonData = GET1(WebUtility.UrlEncode(searchContent));
                    if (jsonData.Length > 0)
                    {
                        var gitCode = JsonConvert.DeserializeObject<RootObject>(jsonData);
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
                            }
                        }
                    }

                    block.Content = block.Content.Remove(0, Math.Min(block.Content.Length, 100));
                }
            }
            if (results.Count == 0)
            {
                return null;
            }
            results = results.OrderBy(r => -r.appearance).ThenBy(r => r.score).ToList();
            var matchResult = results[0];
            var raw = matchResult.html_url.Replace("https://github", "https://raw.githubusercontent").ReplaceFirst("/blob/", "/");
            var fCode = GET(raw);
            return new Result
            {
                Content = fCode,
                FileName = matchResult.html_url,
                Url = raw
            };
        }

        // Function to find index of closing  
        // bracket for given opening bracket.  
        private int GetBracketsIndex(String expression, int index)
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

            Stack st = new Stack();

            for (i = index; i < expression.Length; i++)
            {
                if (expression[i] == '{')
                {
                    st.Push((int)expression[i]);
                }
                else if (expression[i] == '}')
                {
                    st.Pop();
                    if (st.Count == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }



        private int GetNthIndex(string s, char t, int n)
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

        // Returns JSON string
        private string GET(string url)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                json = wc.DownloadString(url);
            }
            return json;
        }

        private List<Block> GetBlocks(String expression)
        {
            List<Block> blocks = new List<Block>();
            Stack<Block> st = new Stack<Block>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '{')
                {
                    st.Push(new Block(i));
                }
                else if (expression[i] == '}')
                {

                    Block block = st.Pop();
                    block.CloseBracket = i;
                    int preBracket = 0;
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

        private string GET1(string url)
        {
            string language = "java";
            Regex regex = new Regex("\\+{2,}");
            url = regex.Replace(url,"+");            
            HttpWebRequest request =
                WebRequest.Create("https://api.github.com/search/code?access_token=517a9c9228a2a58ca163ebb80d58930ceef7776f&q=" + url + " in:file+language:" + language) as HttpWebRequest;
            request.Method = "GET";


            request.Accept = "application/vnd.github.v3.raw+json";

            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return reader.ReadToEnd();

                    }
                }
            }
            catch (WebException ex)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
                {
                    var hh = reader.ReadToEnd();
                    Console.WriteLine(hh);
                    Console.WriteLine(url);
                }
                if (ex.Response.Headers["Retry-After"] != null)
                {
                    var retry = int.Parse(ex.Response.Headers["Retry-After"]);
                    Console.WriteLine($"Wait: {retry}");

                    Thread.Sleep(retry * 1000);
                    return GET1(url);
                }
                else
                {
                    return "";
                }
            }

        }

        // private string GenerateToken()
        // {
        //     var requestParam = new 
        //     {
        //         scopes = new string[1]{"public_repo"},
        //         note = "For request search",
        //         client_id = "7c9c0232060af0d837e4"
        //     };
        // }

    }
}
