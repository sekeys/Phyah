using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPipeline.Diagram
{
    public class PipelineSelector : ISelector
    {
        /// <summary>
        /// 选择器
        /// </summary>
        public List<ISelector> Selectors
        {
            get => default(int);
            set
            {
            }
        }

        public IMatcher Select()
        {
            throw new System.NotImplementedException();
        }

        public void Add(ISelector selector)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ISelector selector)
        {
            throw new System.NotImplementedException();
        }
    }
}