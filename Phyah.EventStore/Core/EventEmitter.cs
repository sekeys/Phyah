using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Phyah.EventStore.Messages;

namespace Phyah.EventStore
{
    public class EventEmitter : IEventEmitter
    {
        public IHub Hub { get => GlobalVariable.Hub; }


        public async void Emit(EventEmitterModel args)
        {
            var context = GlobalVariable.ContextFactory.Create(args);
            var e = GlobalVariable.EventArgsFactory.Create(args);
            if (e == null)
            {
                throw new Exception("400,错误请求，无法初始化事件参数");
            }
            await Hub.BroadcastAsync(args.Url, e);
        }

        public void Wait()
        {
            GlobalVariable.Context.Wait();
        }

        public Task Task => GlobalVariable.Context.Task;
    }
}
