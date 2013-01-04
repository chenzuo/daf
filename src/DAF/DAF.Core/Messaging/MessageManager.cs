using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core;
using DAF.Core.Map;

namespace DAF.Core.Messaging
{
    public class MessageManager
    {
        public static void Publish<T>(T msg)
        {
            var msgSender = IOC.Current.ResolveOptional<IMessageSender>();
            if (msgSender == null)
                return;

            var mappers = IOC.Current.ResolveOptional<IEnumerable<IMessageMapper<T>>>();
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
