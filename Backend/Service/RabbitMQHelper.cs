using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;

namespace Service
{
    public class RabbitMQHelper
    {
        private static ConnectionFactory connectionFactory = null;

        public static void SendMessage(string data, string uri)
        {
            if (connectionFactory == null) {
                connectionFactory = new ConnectionFactory() { Uri = new Uri(uri) };
            }
            using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "sourcecode",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                    channel.BasicPublish(exchange: "",
                                     routingKey: "sourcecode",
                                     basicProperties: null,
                                     body: ParseStringToByte(data));
            }
        }

        private static byte[] ParseIntToByte(int number)
        {
            byte[] intBytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] result = intBytes;
            return result;
        }

        private static byte[] ParseStringToByte(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}
