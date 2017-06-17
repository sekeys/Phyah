using Phyah.EventStore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class EventModel
    {
        /// <summary>
        /// 事件mode
        /// </summary>
        public EventMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 协议类型
        /// </summary>
        public Protocols Protocol
        {
            get;
            set;
        }
    }
}