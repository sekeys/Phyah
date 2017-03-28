﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Observer
{
    public static class ImmutableListExtends

    {
        public static ImmutableList<T> AsImmutableList<T>(this IEnumerable<T> enumerable)
        => new ImmutableList<T>(enumerable);
    }
}