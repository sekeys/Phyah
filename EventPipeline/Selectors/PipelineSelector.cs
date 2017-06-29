using Phyah.Collection;
using Phyah.EventPipeline.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline
{
    using Phyah.EventPipeline.Matchers;
    public class PipelineSelector
    {
        class SelectorCompare : IComparer<ISelector>
        {
            public int Compare(ISelector x, ISelector y)
            {
                return x.Priority - y.Priority;
            }
        }
        /// <summary>
        /// 选择器
        /// </summary>
        private readonly PriorityQueue<ISelector> Selectors;
        protected PipelineSelector()
        {
            Selectors = new PriorityQueue<ISelector>(new SelectorCompare());
        }

        public virtual ISelector Select(Message message)
        {
            var iselectors = Selectors.Clone();
            ISelector iselector = iselectors.Dequeue();
            while (iselectors!=null)
            {
                if (iselector.Select(message))
                {
                    return iselector;
                }
            }
            return null;
        }

        public virtual void Add(ISelector selector)
        {
            Selectors.Enqueue(selector);
        }

        public virtual void Remove(ISelector selector)
        {
            Selectors.Remove(selector);
        }
        private static PipelineSelector selector;
        static object locker = new object();
        public static PipelineSelector Selector
        {
            get
            {
                if (selector == null)
                {
                    lock (locker)
                    {
                        if (selector == null)
                        {
                            selector = new PipelineSelector();
                        }
                    }
                }
                return selector;
            }
        }
    }
}