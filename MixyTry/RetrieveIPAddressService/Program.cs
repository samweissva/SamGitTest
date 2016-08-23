using System;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace RetrieveIPAddressService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new RetrieveIPAddressService();
            while (true)
            {
                Task.Run(()=> { service.RetrieveIpStrategy(); });
                Thread.Sleep(TimeSpan.FromMinutes(service.GetThreadSpan));
            }

        }
    }

    public class RetrieveIPAddressService
    {
        private string ipCache = string.Empty;

        public void RetrieveIpStrategy()
        {
            var result = GetExternalIPAddress(null);
            if (!string.IsNullOrEmpty(ipCache) && ipCache == result)
            {
                return;
            }

            ipCache = result;
            var mail = ComposeMail();
            mail.Subject = $"{ mail.Subject }---{ ipCache }";
            mail.Body = ipCache;
            SendMail(mail);
        }

        private string GetExternalIPAddress(Func<string,string> fun)
        {
            using (var client= new WebClient())
            {
                var respond=client.DownloadString(JsonConfig.GetExternalIpAddressUrl).Trim();
                if (fun!=null)
                {
                    respond = fun(respond);
                }
                return respond;
            }
        }

        private MailMessage ComposeMail()
        {
            var config = JsonConfig;
            MailMessage mail = new MailMessage(config.From, config.To);
            mail.Subject = config.Subject;
            mail.Body = config.Body;
            return mail;
        }

        private bool SendMail(MailMessage mail)
        {
            var result = false;
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    var config = JsonConfig;
                    client.Credentials = new NetworkCredential(config.UserName, config.Password);
                    client.Host =config.Host ;
                    client.Send(mail);
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public int GetThreadSpan
        {
            get
            {
                return JsonConfig.ThreadSpan;
            }
        }

        private JsonConfig jsonConfig = null;
        private JsonConfig JsonConfig
        {
            get
            {
                if (jsonConfig == null)
                {
                    using (StreamReader r = new StreamReader("JsonConfig.json"))
                    {
                        string json = r.ReadToEnd();
                        jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(json);
                    }
                }
                return jsonConfig;
            }
        }
    }

    public class JsonConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string GetExternalIpAddressUrl { get; set; }
        public int ThreadSpan { get; set; }
    }
}
