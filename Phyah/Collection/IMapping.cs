using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Collection
{
    public interface IMapping<T1,T2>
    {
        T2 Map(T1 value);
    }
}
