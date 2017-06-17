using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public interface IEventUpgrader
    {
        /// <summary>
        /// 更新
        /// </summary>
        void Upgrade(EventUpgraderModel args);
    }
}