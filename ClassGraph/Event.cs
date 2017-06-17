using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public abstract class EventArgs
    {
        public object Sender
        {
            get => default(int);
            set
            {
            }
        }
    }
}