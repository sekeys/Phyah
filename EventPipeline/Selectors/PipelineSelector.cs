using Phyah.Collection;
using Phyah.EventPipeline.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline
{
    using Phyah.EventPipeline.Matchers;
    public class PipelineSelector: ISelector
    {
        /// <summary>
        /// 选择器
        /// </summary>
        private readonly List<ISelector> Selectors;
        protected PipelineSelector()
        {
            Selectors = new List<ISelector>();//new PriorityQueue<ISelector>(new SelectorCompare());
        }

        public virtual IEnumerable<IMatcher> Select(Message message)
        {
            var ls = new List<IMatcher>();
            foreach (var iselector in Selectors)
            {
                ls.AddRange(iselector.Select(message));
            }
            return ls;
        }

        public virtual void Add(ISelector selector)
        {
            Selectors.Add(selector);
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