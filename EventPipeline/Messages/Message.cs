using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phyah.EventPipeline.Messages
{
    public class Message
    {
        public Protocols Protocol { get;  set; }
        public Message()
        {
            Data = new DynamicJson();
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
