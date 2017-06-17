using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    using Phyah.EventStore;
    using Phyah.EventStore.Messages;

    public class HubTester
    {
        public void Test()
        {
            GlobalVariable.Hub.Register("simpleAPI.baidu");
            GlobalVariable.Hub.Subscribe("simpleAPI.baidu",)
            var hm = new HttpMessage();
            hm.Data.Url = "http://baidu.com/";
            GlobalVariable.Hub.BroadcastAsync("",new SimpleEventArgs(hm,GlobalVariable.ContextFactory.Create(hm)));
            
        }
        public class SimpleEH : IEventHandler
        {
            public void Processe(EventArgs Event)
            {
                
            }
        }
    }
}
