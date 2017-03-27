using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Extensions
{
    public static class ExceptionThrowExtends
    {
        public static void Throw(this Exception ex)
        {
            throw ex;
        }
        public static void ThrowIfNotNull(this Exception ex)
        {
            if (ex != null)
                throw ex;
        }
    }
}
