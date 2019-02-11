using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ_Quartz_Xml.Tasks;

namespace RabbitMQ_Quartz_Xml.Model
{
    public class Worker
    {
        public IWork task;

        public Worker(IWork task)
        {
            this.task = task;
        }

        public void Work()
        {
            task.Work();
        }
    }
}
