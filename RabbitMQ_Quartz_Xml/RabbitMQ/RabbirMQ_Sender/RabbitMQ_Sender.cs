using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.RabbitMQ.RabbirMQ_Sender
{
    public class RabbitMQ_Sender
    {
        private readonly RabbitMQService _rabbitMQService;
        public RabbitMQ_Sender(string queueName, string message)
        {
            _rabbitMQService = new RabbitMQService();
            using (var connection = _rabbitMQService.GetRabbitMQConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    channel.BasicPublish("", queueName, null, Encoding.UTF8.GetBytes(message));
                    Console.WriteLine("{0} queue'su üzerine, \"{1}\" mesajı yazıldı.", queueName, message);
                }
            }
        }
    }
}
