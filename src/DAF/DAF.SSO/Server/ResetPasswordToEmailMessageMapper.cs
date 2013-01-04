using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Messages;
using DAF.Core.Messaging;
using DAF.Core.Template;

namespace DAF.SSO.Server
{
    public class ResetPasswordToEmailMessageMapper : MessageMapper<ResetPasswordMessage, EmailMessage>
    {
        private ITemplateEngine engine;

        public ResetPasswordToEmailMessageMapper(ITemplateEngine engine)
        {
            this.engine = engine;
        }

        protected override EmailMessage MapTo(ResetPasswordMessage obj)
        {
            TemplateProperty templateProperty = new TemplateProperty()
            {
                ClientId = obj.ClientId,
                Language = obj.Language,
                Year = DateTime.Today.Year,
                BizGroup = "DAF.SSO",
                TemplateName = "ResetPassword.cshtml"
            };

            var result = engine.LoadTemplate(obj, templateProperty);

            EmailMessage mail = new EmailMessage()
            {
                To = new string[] { obj.Email },
                Subject = Resources.Locale(o => o.ResetPasswordTitle),
                Body = result.Body
            };

            return mail;
        }
    }
}
