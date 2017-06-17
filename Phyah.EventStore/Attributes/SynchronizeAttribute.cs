using Phyah.EventStore.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore.Attributes
{
    public class SynchronizeAttribute:Attribute
    {
        /// <summary>
        /// 是否异步
        /// </summary>
        public SynchronizeTypes Synchronize
        {
            get;
            set;
        }
        public SynchronizeAttribute(SynchronizeTypes synchronize= SynchronizeTypes.Sync)
        {
            this.Synchronize = synchronize;
        }
    }
}