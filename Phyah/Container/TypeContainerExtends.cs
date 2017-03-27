

namespace Phyah.Container
{
    using Phyah.Chain;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public static class TypeContainerExtends
    {
        public static object Fetch<T>(this TypeContainer container)
        {
            return Constructor.Construct<T>();
        }
        public static object Fetch(Type baseType)
        {
            return Constructor.Construct(baseType);
        }


        public static void Fetch<T>(ActionChain<T> chain)
        {
            Constructor.Construct<T>(chain);
        }
    }
}
