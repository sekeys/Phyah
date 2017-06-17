using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public interface IEventHandler
    {
        /// <summary>
        /// 事件
        /// </summary>
        EventArgs Event { get; set; }

        /// <summary>
        /// 处理
        /// </summary>
        void Processe();
    }
}