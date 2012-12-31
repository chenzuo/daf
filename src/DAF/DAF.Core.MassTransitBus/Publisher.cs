using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;
using Autofac;
using DAF.Core;

namespace DAF.Core.MassTransitBus
{
    public class Publisher : ProviderBaseWithLogger, IPublisher
    {
        private IServiceBus bus;
        //private ConcurrentDictionary<Type, List<UnsubscribeAction>> unsubscribeActions;

        public Publisher(IServiceBus bus)
            : base()
        {
            this.bus = bus;
            //this.unsubscribeActions = new ConcurrentDictionary<Type, List<UnsubscribeAction>>();
        }

        public void Publish<T>(T msg) where T : class
        {
            //EnsureSubscribers<T>();
            bus.Publish<T>(msg);
        }

        //private void EnsureSubscribers<T>() where T : class
        //{
        //    var msgType = typeof(T);
        //    if (unsubscribeActions.ContainsKey(msgType))
        //        return;

        //    List<UnsubscribeAction> actions = new List<UnsubscribeAction>();
        //    var subs = IOC.Current.ResolveOptional<IEnumerable<ISubscriber<T>>>();
        //    if (subs != null)
        //    {
        //        foreach (var sub in subs)
        //        {
        //            UnsubscribeAction act = bus.SubscribeInstance<CommonConsumer<T>>(new CommonConsumer<T>(sub));
        //            actions.Add(act);
        //        }
        //    }

        //    unsubscribeActions.AddOrUpdate(msgType, actions, (t, a) => a);
        //}
    }
}
