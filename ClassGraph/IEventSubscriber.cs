using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public interface IEventSubscriber
    {
        /// <summary>
        /// 订阅
        /// </summary>
        void Subscribe(EventSubscriberModel args);
        /// <summary>
        /// 取消订阅
        /// </summary>
        void Unsubscribe(EventSubscriberModel args);
    }
}