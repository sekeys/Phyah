using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public interface IEventEmitter
    {
        IHub Hub { get; set; }

        /// <summary>
        /// 发射事件
        /// </summary>
        void Emit(EventEmitterModel args);
    }
}