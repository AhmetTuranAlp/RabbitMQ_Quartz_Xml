using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ_Quartz_Xml.Model;
using RabbitMQ_Quartz_Xml.Tasks;

namespace RabbitMQ_Quartz_Xml
{
    class Program
    {
        private static string[] operations = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        static void Main(string[] args)
        {
            bool manuel = false;
            Worker Worker = null;
            if (manuel)
            {
                Worker = new Worker(new GeneralMaintance());
                Worker.Work();
            }
            else
            {
                Console.Title = "RabbitMQ";

                string workMode = string.Empty;
                do
                {
                    workMode = PrintModeTable();
                }
                while (!operations.Contains(workMode));

                Console.Clear();
                switch (workMode)
                {
                    case "1": Console.Title = "Xml Sender"; Worker = new Worker(new XmlSender()); break;
                    case "2": Console.Title = "Xml Recieve"; Worker = new Worker(new XmlRecieve()); break;
                }

                Worker.Work();
                Console.ReadKey();
            }
        }

        private static string PrintModeTable()
        {
            Console.Clear();

            int tableLeft = 40;
            int tableRight = 30;
            Console.WriteLine("Console Version : 1\n\n");
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", getSeperator(tableLeft), getSeperator(tableRight)));
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", "Çalışma Modu", "Girilmesi Gereken Değer"));
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", getSeperator(tableLeft), getSeperator(tableRight)));
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", "Xml Sender", "1"));
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", "Xml Recieve", "2"));
            Console.WriteLine(String.Format("|{0," + tableLeft + "}|{1," + tableRight + "}|", getSeperator(tableLeft), getSeperator(tableRight)));

            Console.WriteLine("\n\nLütfen çalışma modu için tablodan bir değer giriniz:");
            return Console.ReadLine();
        }

        public static string getSeperator(int length)
        {
            string seperator = string.Empty;
            for (int i = 0; i < length; i++)
            {
                seperator += "-";
            }
            return seperator;
        }
    }
}
