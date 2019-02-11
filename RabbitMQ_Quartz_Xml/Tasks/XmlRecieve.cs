using RabbitMQ_Quartz_Xml.Model;
using RabbitMQ_Quartz_Xml.RabbitMQ.RabbitMQ_Recieve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.Tasks
{
    public class XmlRecieve : IWork
    {
        private static string _queueName = "Xml Update";
        private static RabbitMQ_Recieve _reciver;
        public void Work()
        {
            _reciver = new RabbitMQ_Recieve(_queueName);
        }
    }
}
