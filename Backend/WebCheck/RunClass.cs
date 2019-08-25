using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebCheck
{
    class RunClass
    {
        public static void Main(string[] args)
        {
            // Display the number of command line arguments:
            WebCheck check = new Github();
            var connectionFactory = RabbitMQHelper.connectionFactory;
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "webcheck",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    System.Console.WriteLine("Go receive");
                    Console.Clear();
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var messageContent = JsonConvert.DeserializeObject<MessageObject>(message);
                    var source = GET(messageContent.Url);
                    WebCheck checker = new Github();
                    Submission submission = null;
                    List<CodeDetail> codeDetails = new List<CodeDetail>();
                    using (var context = new MyContext())
                    {
                        try {
                            submission = context.SourceCode.Where(s => s.Id == Guid.Parse(messageContent.Id)).FirstOrDefault();
                            System.Console.WriteLine("Before context");
                            codeDetails = context.CodeDetails.Where(c => c.SourceCodeId == submission.DocumentId).ToList();
                        }
                        catch(Exception ex)
                        {
                            System.Console.WriteLine(ex.Source);
                            System.Console.WriteLine(ex.Data);
                            System.Console.WriteLine(ex.StackTrace);
                        }
                    }

                    System.Console.WriteLine("Receive web search request for file " + submission.DocumentName);

                    System.Console.WriteLine("Start web search for file " + submission.DocumentName);
                    var result = checker.Check(codeDetails);
                    System.Console.WriteLine("Done web search for file " + submission.DocumentName);
                    System.Console.WriteLine();
                    if (result == null)
                    {
                        System.Console.WriteLine("There is no similar file on the Internet for file " + submission.DocumentName);
                        using (var context = new MyContext())
                        {

                            submission.Status = SourceCodeStatus.NOSIMILAR;
                            context.Update(submission);
                            context.SaveChanges();
                        }
                        return;
                    }
                    System.Console.WriteLine("The similar file to file " + submission.DocumentName + " has URL " + result.FileName);
                    Submission sourceCode = new Submission{
                            Status = SourceCodeStatus.PROCESSING,
                            Type = SourceCodeType.WEB,
                            DocumentName = result.FileName,
                            FileUrl = result.Url,
                            UploadDate = DateTime.UtcNow.AddHours(7)
                    };
                    Console.WriteLine("Done search");

                    using (var context = new MyContext())
                    {
                        var findResult = context.SourceCode.Where(s => s.FileUrl == result.Url).FirstOrDefault();
                        if (findResult == null)
                        {
                            context.SourceCode.Add(sourceCode);
                            context.SaveChanges();
                        }
                        else
                        {
                            sourceCode = findResult;
                        }
                    }
                    var newMessage = JsonConvert.SerializeObject(new
                    {
                        id = sourceCode.Id,
                        webCheck = false,
                        peerCheck = false,
                        continueWebCheck = messageContent.Id
                    });
                    RabbitMQHelper.SendMessage(newMessage);
                };
                System.Console.WriteLine("Done setup");
                channel.BasicConsume("webcheck", autoAck: true, consumer: consumer);
                Console.ReadKey();
            }
        }

        private static string GET(string url)
        {
            string json = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                json = wc.DownloadString(url);
            }
            return json;
        }
    }
}
