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
                    WebCheck checker = new Github();
                    Console.WriteLine("Go receive");
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var messageContent = JsonConvert.DeserializeObject<MessageObject>(message);
                    var source = GET(messageContent.Url);
                    var result = checker.Check(source);
                    Console.WriteLine("Done search");
                    Submission sourceCode = new Submission{
                            Status = SourceCodeStatus.PENDING,
                            Type = SourceCodeType.WEB,
                            DocumentName = result.FileName,
                            FileUrl = result.Url,
                            UploadDate = DateTime.Now
                    };
                    using (var context = new MyContext())
                    {
                        var findResult = context.SourceCode.Where(s => s.FileUrl == result.Url).FirstOrDefault();
                        if(findResult == null)
                        {
                            context.SourceCode.Add(sourceCode);
                            context.SaveChanges();
                        }
                        else{
                            sourceCode = findResult;
                        }
                    }
                    Console.WriteLine("Done insert");
                    var newMessage = JsonConvert.SerializeObject(new {
                        id = sourceCode.Id,
                        webCheck = false,
                        peerCheck = false,
                        continueWebCheck = messageContent.Id
                    });
                    Console.WriteLine(newMessage);
                    RabbitMQHelper.SendMessage(newMessage);
                };
                Console.WriteLine("Done set up");
                channel.BasicConsume("webcheck", autoAck: true, consumer);
                Console.WriteLine("Open");
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
