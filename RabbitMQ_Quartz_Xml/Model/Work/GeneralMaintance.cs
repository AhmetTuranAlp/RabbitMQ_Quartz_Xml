using Newtonsoft.Json;
using RabbitMQ_Quartz_Xml.Model;
using RabbitMQ_Quartz_Xml.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml
{
    public class GeneralMaintance : IWork
    {
        public void Work()
        {
            try
            {
                Worker Worker = null;
                string data = "";
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"Model.txt", true);
                data = sr.ReadToEnd();
                sr.Close();
                DigiCommonEntegrayon newTask = JsonConvert.DeserializeObject<DigiCommonEntegrayon>(data);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Model.txt"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Model.txt");
                }
                if (newTask != null)
                {
                    switch (newTask.BotIslemTipi)
                    {
                        case BotIslemTipi.RabbitMQSender:
                             Worker = new Worker(new XmlSender());
                            break;
                        case BotIslemTipi.RabbitMQRecieve:
                            Worker = new Worker(new XmlRecieve());
                            break;
                        default:
                            break;
                    }
                    Worker.Work();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
