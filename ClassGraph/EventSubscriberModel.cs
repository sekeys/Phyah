using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class EventSubscriberModel : EventModel
    {
        /// <summary>
        /// 订阅者
        /// </summary>
        public string Subscriber
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 订阅者路径
        /// </summary>
        public string SubscriberUrl
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 是否是卸载
        /// </summary>
        public bool IsUnsubscribe
        {
            get => default(int);
            set
            {
            }
        }
    }
}