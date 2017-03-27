
namespace Phyah.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Concurrent;
    public class ChannelEmitter
    {
        public System.Collections.Concurrent.ConcurrentDictionary<string, ChannelReceptor> Emitter;
        protected ChannelEmitter()
        {
            Emitter = new System.Collections.Concurrent.ConcurrentDictionary<string, ChannelReceptor>();
        }
        public void Emit(ChannelEvent e)
        {
            e.Executable.Execute();
        }

        public void Emit<T>(ChannelEvent e)
        {

        }
        public void Emit(string eventName,ChannelEvent e)
        {

        }
        public ChannelEmitter  Subscribe(string name,ChannelReceptor receptor)
        {
            return this;
        }
        public ChannelEmitter Subscribe(ChannelReceptor receptor)
        {
            return this;
        }
    }
}
