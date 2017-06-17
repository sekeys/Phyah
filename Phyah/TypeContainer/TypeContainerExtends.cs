

namespace Phyah.TypeContainer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public static class TypeContainerExtends
    {
        public static T Construct<T>(this ITypeContainer container)
        {
            return Constructor.Construct<T>();
        }
        public static object Construct(this ITypeContainer container,Type baseType)
        {
            return Constructor.Construct(baseType);
        }
        
    }
}
