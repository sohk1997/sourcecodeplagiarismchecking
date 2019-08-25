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
        public Result Check(List<CodeDetail> codeDetails)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            List<Item> results = new List<Item>();
            int countRequest = 0;
            foreach (var codeDetail in codeDetails)
            {
                var block = new Block
                {
                    Content = codeDetail.MethodString
                };
                string[] words = block.Content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int index = 0;
                while (index < words.Length)
                {
                    string content = "";
                    while (index < words.Length && (content.Length + WebUtility.UrlEncode(" " + words[index]).Length <= 100 || content.Length == 0))
                    {
                        if (content.Length > 0)
                        {
                            content = content + WebUtility.UrlEncode(" " + words[index]);
                        }
                        else
                        {
                            content = WebUtility.UrlEncode(words[index]);
                        }
                        index++;
                    }
                    block.Content = content;
                    var searchContent = block.Content;

                    countRequest++;
                    string jsonData = SearchCode(searchContent);
                    if (jsonData.Length > 0)
                    {
                        var gitCode = JsonConvert.DeserializeObject<RootObject>(jsonData);
                        foreach (var item in gitCode.items)
                        {
                            //Check if item is exist in result
                            if (results.Find(it => it.html_url == item.html_url) != null)
                            {
                                results.Find(it => it.html_url == item.html_url).appearance++;
                                results.Find(it => it.html_url == item.html_url).score += item.score;
                            }
                            else
                            {
                                results.Add(item);
                            }
                        }
                    }
                    System.Console.WriteLine("Done for searching next block");
                }
            }
            if (results.Count == 0)
            {
                return null;
            }

            //The first Item in Result List is the most similarity case
            results = results.OrderBy(r => -r.appearance).ThenBy(r => -(r.score / countRequest)).ToList();
            // for(int i = 0 ; i < Math.Min(results.Count, 5) ; i++)
            // {
            //     System.Console.WriteLine(results[i].html_url + " " + results[i].appearance + " " + results[i].score);
            // }
            var matchResult = results[0];
            //Parse url code on github to raw file
            var raw = matchResult.html_url.Replace("https://github", "https://raw.githubusercontent").ReplaceFirst("/blob/", "/");
            var fCode = GetCode(raw);
            return new Result
            {
                Content = fCode,
                FileName = matchResult.html_url,
                Url = raw
            };
        }

        // Returns JSON string
        private string GetCode(string url)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                json = wc.DownloadString(url);
            }
            return json;
        }

        //Parse String to list of block code
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
                    if (st.Count < 2)
                    {
                        blocks.Add(block);
                    }
                }
            }

            return blocks;
        }

        private string SearchCode(string url)
        {
            string language = "java";
            HttpWebRequest request =
                WebRequest.Create("https://api.github.com/search/code?client_id=7c9c0232060af0d837e4&client_secret=148442321f490cec5846c87d69fc52465cb394fd&access_token=3ceca66dbf12323bb31bbfb5cf7f5926313afa45&q=" + url + " in:file language:" + language) as HttpWebRequest;
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
                if (ex.Response.Headers["Retry-After"] != null)
                {
                    var retry = int.Parse(ex.Response.Headers["Retry-After"]);
                    System.Console.WriteLine($"Exceed maximum request. Please wait: {retry} second to continue");

                    Thread.Sleep(retry * 1000);
                    return SearchCode(url);
                }
                else
                {
                    // string mess = "";
                    // using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
                    // {
                    //     mess = reader.ReadToEnd();

                    // }
                    // System.Console.WriteLine(url);
                    // System.Console.WriteLine(mess);
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
