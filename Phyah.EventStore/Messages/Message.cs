using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phyah.EventStore.Messages
{
    public class Message
    {
        public EventContext Context { get; internal protected set; }
        public Message(EventContext context)
        {
            Context = context;
            Data = new DynamicJson();
        }
        public Message()
        {
        }
        public string Msg { get; set; }
        public string Guid { get; set; }
        public dynamic Data { get;private set; }
        public string Verb { get; set; }
        public void Deserialize(string data)
        {
            this.Data = DynamicData.Deserializes(data);
        }
        public string Serialize()
        {
            return this.Data.Serialize();
        }
        public void Deserialize(Stream stream)
        {

        }
        public Stream SerializeStream()
        {
            return null;
        }
    }
}
