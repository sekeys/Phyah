using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
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
    }
}