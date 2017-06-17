using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class EventRegisterModel : EventModel
    {
        /// <summary>
        /// 事件处理类型
        /// </summary>
        public string Handler
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 事件处理dll获取的URL
        /// </summary>
        public string HandlerUrl
        {
            get => default(int);
            set
            {
            }
        }
    }
}