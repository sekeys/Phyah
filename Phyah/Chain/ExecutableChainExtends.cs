using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Chain
{
    public static class ExecutableChainExtends
    {
        #region
        public  static ExecutableChain Action(this ExecutableChain chain, Action action)
        {
            chain.Enqueue(new ActionExecutable(action));
            return chain;
        }
        public static ExecutableChain Action<T>(this ExecutableChain chain, Action<T> action,T parameter)
        {
            chain.Enqueue(new ActionExecutable<T>(action,parameter));
            return chain;
        }
        public static ExecutableChain Action<T,T1>(this ExecutableChain chain, Action<T,T1> action,params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T,T1>(action,parameter));
            return chain;
        }
        public static ExecutableChain Action<T, T1, T2>(this ExecutableChain chain, Action<T,T1,T2> action, params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T,T1,T2>(action, parameter));
            return chain;
        }
        public static ExecutableChain Action<T, T1, T2, T3>(this ExecutableChain chain, Action<T, T1, T2, T3> action, params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T, T1, T2, T3>(action, parameter));
            return chain;
        }
        public static ExecutableChain Action<T, T1, T2, T3, T4>(this ExecutableChain chain, Action<T,T1, T2, T3, T4> action, params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T, T1, T2, T3, T4>(action, parameter));
            return chain;
        }
        public static ExecutableChain Action<T, T1, T2, T3, T4, T5>(this ExecutableChain chain, Action<T, T1, T2, T3, T4, T5> action, params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T, T1, T2, T3, T4, T5>(action, parameter));
            return chain;
        }
        public static ExecutableChain Action<T, T1, T2, T3, T4, T5, T6>(this ExecutableChain chain, Action<T, T1, T2, T3, T4, T5, T6> action, params object[] parameter)
        {
            chain.Enqueue(new ActionExecutable<T, T1, T2, T3, T4, T5, T6>(action, parameter));
            return chain;
        }
        #endregion
    }
}
