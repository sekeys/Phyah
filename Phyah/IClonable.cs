using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah
{
    public interface IClonable<T>
    {
        T Clone();
    }
}
