using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public abstract class EventHub : IHub
    {
        /// <summary>
        /// 执行器
        /// </summary>
        public Executor Schedule
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 工作
        /// </summary>
        public Executor Worker
        {
            get => default(int);
            set
            {
            }
        }

        public int TaskCompletionSource
        {
            get => default(int);
            set
            {
            }
        }
    }
}