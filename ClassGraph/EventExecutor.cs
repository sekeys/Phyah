using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public abstract class EventExecutor
    {
        public int TaskCompletionSource
        {
            get => default(int);
            set
            {
            }
        }

        public int Task
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 等待完成
        /// </summary>
        public void Wait()
        {
            throw new System.NotImplementedException();
        }
    }
}