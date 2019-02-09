using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Quartz_Xml.RabbitMQ.RabbitMQ_Recieve
{
    public class WebClientWithCookie : WebClient
    {
        public WebClientWithCookie()
        {
            CookieContainer = new CookieContainer();
            this.Encoding = Encoding.UTF8;
        }
        public CookieContainer CookieContainer { get; private set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            this.Encoding = Encoding.UTF8;
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            request.Timeout = 600000;
            return request;
        }
    }
}
