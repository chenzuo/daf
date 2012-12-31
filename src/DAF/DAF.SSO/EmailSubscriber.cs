using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.SSO.Server;
using DAF.Core.Messaging;
using DAF.Core.Template;

namespace DAF.SSO
{
    public class EmailSubscriber : ISubscriber<ResetPasswordMessage>
    {
        private IEmailSender emailSender;
        private ITemplateEngine engine;

        public EmailSubscriber(IEmailSender emailSender, ITemplateEngine engine)
        {
            this.emailSender = emailSender;
            this.engine = engine;
        }

        public void OnReceive(ResetPasswordMessage msg)
        {
            TemplateProperty templateProperty = new TemplateProperty()
            {
                ClientId = msg.ClientId,
                Language = msg.Language,
                Year = DateTime.Today.Year,
                BizGroup = "DAF.SSO",
                TemplateName = "ResetPassword.cshtml"
            };

            var result = engine.LoadTemplate(msg, templateProperty);
            if (result != null)
            {
                emailSender.Send(null, new string[] { msg.Email }, null, DAF.SSO.Resources.Locale(o => o.ResetPasswordTitle), result.Body, null);
            }
        }
    }
}
