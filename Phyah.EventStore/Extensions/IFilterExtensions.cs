using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Extensions
{
    public static class IFilterExtensions
    {
        public static bool IsAuthenticationFilter(this IFilter filter)
        {
            return filter is EventDescriptor.AuthenticationFilter;
        }
    }
}
