using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventHub.Attributes
{
    public class AsynchronousAttribute:System.Attribute
    {
        public bool Asynchronous { get; set; } = true;
        public AsynchronousAttribute()
        {

        }
        public AsynchronousAttribute(bool async)
        {
            this.Asynchronous = async;
        }
    }
}
