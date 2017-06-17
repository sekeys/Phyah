using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Messages
{
    public class DynamicXml : DynamicData
    {
        public DynamicXml(bool ignorecase):base(ignorecase)
        {

        }
        public DynamicXml():this(false)
        {
        }
        public override void Deserialize(string value)
        {
            throw new NotImplementedException();
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
