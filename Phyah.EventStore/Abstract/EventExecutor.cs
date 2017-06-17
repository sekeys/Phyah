using Phyah.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.EventStore
{
    public abstract class EventExecutor
    {
        public IEnumerable<EventDescriptor> Descriptors { get; set; }
        public EventExecutor(EventContext context)
        {
            Context = context;
        }
        public EventContext Context { get; protected set; }
        public TaskCompletionSource TaskCompletionSource
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            var task =new List<Task>();
            foreach(var item in Descriptors)
            {
                if (item.Asynchronous == Enumerate.SynchronizeTypes.Sync)
                {
                    item.Handler.Processe(Context.Event);
                }
                else
                {
                    task.Add(Task.Run(() => {
                        item.Handler.Processe(Context.Event);
                    }));
                }
            }
            Task.WaitAll(task.ToArray());
            Context.SetResult();
        }

        public void Cancel()
        {
            Context.Cancel();
        }
        
    }
}