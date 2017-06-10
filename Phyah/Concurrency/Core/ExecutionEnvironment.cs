using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    using System;

    public static class ExecutionEnvironment
    {
        [ThreadStatic]
        static IExecutor currentExecutor;

        public static bool TryGetCurrentExecutor(out IExecutor executor)
        {
            executor = currentExecutor;
            return executor != null;
        }

        internal static void SetCurrentExecutor(IExecutor executor) => currentExecutor = executor;
    }
}
