using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class EventDescriptor
    {
        private IEventHandler _Handler;

        /// <summary>
        /// 事件处理
        /// </summary>
        public IEventHandler Handler
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 是否异步
        /// </summary>
        public bool Asynchronous
        {
            get => default(int);
            set
            {
            }
        }

        public string ID
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 事件请求时间限制
        /// </summary>
        public int Timeout
        {
            get => default(int);
            set
            {
            }
        }

        public void Match()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 栓选
        /// </summary>
        public void Filter(EventDescriptor description, EventArgs args, EventContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}