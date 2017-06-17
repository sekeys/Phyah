using Phyah.Concurrency;
using Phyah.EventStore.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Phyah.EventStore
{
    public abstract class EventContext
    {
        protected EventContext()
        {
            ContextData = new DynamicJson();
        }
        public EventContext(string eventName, Messages.Message message) :this()
        {
            this.EventName = eventName;
            this.Thread = Thread.CurrentThread;
            this.Message = message;
            this.Event = GlobalVariable.EventArgsFactory.Create(Message, this);
        }
        public EventContext(string eventName, Messages.Message message,Thread thread) : this()
        {
            this.EventName = eventName;
            this.Thread = thread;
            this.Message = message;
            this.Event = GlobalVariable.EventArgsFactory.Create(Message, this);
        }
        public object Result { get; set; }
        public System.Threading.Thread Thread { get;private set; }
        internal protected TaskCompletionSource TaskCompletionSource { get;private set; }
        public Task Task => TaskCompletionSource.Task;
        public void Wait()
        {
            Task.Wait();
        }
        public void Complete()
        {
            TaskCompletionSource.TryComplete();
        }
        public void Cancel()
        {
            TaskCompletionSource.TrySetCanceled();
        }
        public void Exception(Exception ex)
        {
            TaskCompletionSource.TrySetException(ex);
        }
        internal void SetResult(int result)
        {
            TaskCompletionSource.TrySetResult(result);
        }
        internal void SetResult()
        {
            TaskCompletionSource.TrySetResult(1);
        }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName
        {
            get;
            private set;
        }

        /// <summary>
        /// 事件
        /// </summary>
        public EventArgs Event
        {
            get;
            private set;
        }

        /// <summary>
        /// 存储的值
        /// </summary>
        public dynamic ContextData
        {
            get;
            private set;
        }

        public string Guid
        {
            get;
            private set;
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        public TimeSpan ExecuteInterval
        {
            get => TimeSpan.Zero;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        //public IUser User
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 传入数据信息
        /// </summary>
        public Messages.Message Message
        {
            get;
            private set;
        }
    }
}