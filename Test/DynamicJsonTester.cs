using Phyah.EventStore.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class DynamicJsonTester
    {
        public void Test()
        {
            var d = new DynamicJson();
            d.Deserialize(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                id = "12321",
                bytea = 123,
                b = true,
                array = new object[] { 123, true, 123
                ,new  { id=1,b=123,c=new  { d=1 } } }
            }));
        }
    }
}
