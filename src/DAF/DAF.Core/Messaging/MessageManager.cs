using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;
using DAF.Core;
using DAF.Core.Map;

namespace DAF.Core.Messaging
{
    public class MessageManager
    {
        public static void Publish<T>(T msg)
        {
            var msgSender = IocInstance.Container.ResolveOptional<IMessageSender>();
            if (msgSender == null)
                return;

            var mappers = IocInstance.Container.ResolveOptional<IEnumerable<IMessageMapper<T>>>();
            if (mappers == null || mappers.Count() <= 0)
            {
                try
                {
                    msgSender.Send(msg);
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex, Resources.Locale(o => o.MessageSendingFailed));
                }
            }
            else
            {
                foreach (var mapper in mappers)
                {
                    try
                    {
                        var obj = mapper.Map(msg);
                        msgSender.Send(obj);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error(ex, Resources.Locale(o => o.MessageSendingFailed));
                    }
                }
            }
        }
    }
}
