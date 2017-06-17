using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public abstract class EventContext
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 事件
        /// </summary>
        public EventArgs Event
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 存储的值
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get => default(int);
            set
            {
            }
        }

        public string Guid
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        public TimeSpan ExecuteInterval
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public IUser User
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 传入数据信息
        /// </summary>
        public EventModel Model
        {
            get => default(int);
            set
            {
            }
        }
    }
}