using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MCQ.Code
{
    public static class EmailHelper
    {
        public static void Send(string to, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("support@cardrivingtheory.co.uk");
            msg.To.Add(new MailAddress(to));
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;


            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtpout.secureserver.net";
            client.Port = 25;
            //client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpLogin"], ConfigurationManager.AppSettings["SmtpPassword"]);
            client.Timeout = 20000;
          
                client.Send(msg);

        }

    }
}