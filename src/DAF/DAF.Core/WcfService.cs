using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace DAF.Core
{
    public class WcfService
    {
        public static ChannelFactory<T> CreateChannel<T>(string endpointConfigName)
        {
            return new ChannelFactory<T>(endpointConfigName);
        }
    }

    public static class ChannelFactoryExtensions
    {
        public static void Call<TT>(this ChannelFactory<TT> channelFactory, Action<TT> action)
        {
            try
            {
                var provider = channelFactory.CreateChannel();
                action(provider);
                if (channelFactory.State != CommunicationState.Faulted)
                {
                    channelFactory.Close();
                }
            }
            finally
            {
                channelFactory.Abort();
            }
        }
    }

}
