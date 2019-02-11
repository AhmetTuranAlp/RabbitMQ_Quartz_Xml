using RabbitMQ_Quartz_Xml.Model;
using RabbitMQ_Quartz_Xml.Model.Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.Tasks
{
    public class XmlSender : IWork
    {
        public void Work()
        {
            XmlDataTrigger trigger = new XmlDataTrigger();
            trigger.Tetikle();
        }
    }
}
