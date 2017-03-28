
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
            foreach(var item in Emitter.Values)
               Task.Run(()=>{ item.Accept(e);});
        }

        public void Emit<T>(ChannelEvent e)
        {
            foreach(var item in Emitter.Values)
                if(item is T)
                    Task.Run(()=>{ item.Accept(e);});
        }
        public void Emit(string eventName,ChannelEvent e)
        {
            if(Emitter.Contains(eventName)){
                Emitter[eventName].Accept(e);
            }
        }
        public ChannelEmitter  Subscribe(string name,ChannelReceptor receptor)
        {
            if(Emitter.Contains(name)){
                Emitter[name]=receptor;
            }else{
                Emitter.Add(name,receptor);
            }
            return this;
        }
        public ChannelEmitter Subscribe(ChannelReceptor receptor)
        {
            string str=nameof(Receptor);
            if(str.EndsWith("ChannelReceptor")){
                str=str.Substring(0,str.Length-15);
            }else if(str.EndsWith("Receptor")){
                str=str.Substring(0,str.Length-8);
            }
            return Subscribe(str,receptor);
        }
    }
}
