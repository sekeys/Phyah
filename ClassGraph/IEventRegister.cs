using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public interface IEventRegister
    {
        IHub Hub { get; set; }

        /// <summary>
        /// 注册事件
        /// </summary>
        void Register(EventRegisterModel args);
    }
}