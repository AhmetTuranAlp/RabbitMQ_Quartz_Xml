using Quartz;
using RabbitMQ_Quartz_Xml.RabbitMQ.RabbirMQ_Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.Model.Quartz
{
    public class XmlDataJob : IJob
    {
        private static string _queueName = "Xml Update";
        private static RabbitMQ_Sender _sender;

        public void Execute(IJobExecutionContext context)
        {
            _sender = new RabbitMQ_Sender(_queueName, "ahmet");
        }
    }
}
