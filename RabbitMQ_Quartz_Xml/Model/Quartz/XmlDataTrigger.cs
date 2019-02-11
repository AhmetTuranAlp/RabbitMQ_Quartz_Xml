using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.Model.Quartz
{
    public class XmlDataTrigger
    {
        private IScheduler Baslat()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = schedFact.GetScheduler();
            if (!sched.IsStarted)
                sched.Start();
            return sched;
        }

        public void Tetikle()
        {
            IScheduler sched = Baslat();
            IJobDetail Job = JobBuilder.Create<XmlDataJob>().WithIdentity("Job", null).Build();
            ITrigger TriggerJob = TriggerBuilder.Create()
                          .WithIdentity("Job")
                          //.StartAt(DateBuilder.EvenMinuteDate(null))
                          .WithCronSchedule("00 0/1 * ? * *") // her dakika başı
                                                              //.WithCronSchedule("00 00 * ? * *") // her saat başı
                          .Build();

            sched.ScheduleJob(Job, TriggerJob);
        }
    }
}
