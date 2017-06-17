using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.EventStore.Transport
{
    public abstract class Transport
    {
        public Messages.Message Message { get; set; }
        public abstract void Send();
        public abstract Task<Messages.Message> SendAsync();
        public abstract void Send(Action<Messages.Message, EventContext> callback);
        public abstract void SendNoWait();
    }
}
