using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    public interface IHandler<T>
    {
        void Handle(T value); 
    }
}
