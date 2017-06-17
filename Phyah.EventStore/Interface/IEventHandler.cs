using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore
{
    public interface IEventHandler
    {

        /// <summary>
        /// 处理
        /// </summary>
        void Processe(EventArgs Event);
    }
}