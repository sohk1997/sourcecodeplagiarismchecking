using System;
using System.Text;
using RabbitMQ;
using RabbitMQ.Client;

namespace Service
{
    public class RabbitMQHelper
    {
        private static ConnectionFactory connectionFactory = new ConnectionFactory() { Uri = new Uri("amqp://tojbmlqr:fO9Tcz9MRDmzl1J0B_56LBT3BO1VPxWB@mustang.rmq.cloudamqp.com/tojbmlqr") };

        public static void SendMessage(string id)
        {
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
                                     body: ParseStringToByte(id));
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
