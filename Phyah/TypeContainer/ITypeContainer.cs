﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.TypeContainer
{
    public interface ITypeContainer
    {
        void Inject<baseType, ImpType>();
        void Inject(Type interType, Type impType);
        void Inject<T>(Type impType);
        Type Fetch(Type type);
        IEnumerable<Type> FetchInterface();
        IEnumerable<KeyValuePair<Type, Type>> Fetch();
        Type Fetch<T>();
    }
}
