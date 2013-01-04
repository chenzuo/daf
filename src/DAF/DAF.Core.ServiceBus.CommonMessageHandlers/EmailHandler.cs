using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Web;
using NServiceBus;
using DAF.Core;
using DAF.Core.Messages;
using DAF.Core.Messaging;
using DAF.Core.Template;

namespace DAF.Core.ServiceBus.CommonMessageHandlers
{
    public class EmailHandler : IHandleMessages<EmailMessage>
    {
        private string server;
        private string account;
        private string password;

        public EmailHandler()
        {
            server = ConfigurationManager.AppSettings["smtpServer"];
            account = ConfigurationManager.AppSettings["smtpAccount"];
            password = ConfigurationManager.AppSettings["smtpPassword"];
        }

        public void Handle(EmailMessage message)
        {
            if (string.IsNullOrEmpty(message.From))
                message.From = account;
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(message.From),
                Subject = message.Subject,
                Body = message.Body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            foreach (var to in message.To)
            {
                mail.To.Add(new MailAddress(to));
            }
            if (message.CC != null)
            {
                foreach (var cc in message.CC)
                {
                    mail.CC.Add(new MailAddress(cc));
                }
            }
            if (message.Attachments != null)
            {
                foreach (var f in message.Attachments)
                {
                    mail.Attachments.Add(new Attachment(f));
                }
            }

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = server;
                smtp.Credentials = new NetworkCredential(account, password);
                object userState = mail;
                smtp.SendAsync(mail, userState);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Sending mail failure.");
            }
        }
    }
}
