

namespace Phyah.Javascript
{
    using ChakraCore.NET;
    using Phyah.Thread;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class JavascriptScope
    {
        public static void Register<T>(string scope, T obj)
        {
            Accessor<ChakraContext>.Current.GlobalObject.WriteProperty<T>(scope, obj);
        }
        public static void RegisterFunc<T1>(string scope, Func<T1> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1>(scope, func);
        }

        public static void RegisterFunc<T1,T2>(string scope, Func<T1,T2> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2>(scope, func);
        }
        public static void RegisterFunc<T1, T2, T3>(string scope, Func<T1, T2, T3> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2, T3>(scope, func);
        }
        public static void RegisterFunc<T1, T2, T3, T4>(string scope, Func<T1, T2, T3, T4> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2, T3, T4>(scope, func);
        }
        public static void RegisterFunc<T1, T2, T3, T4, T5>(string scope, Func<T1, T2, T3, T4, T5> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2, T3, T4, T5>(scope, func);
        }

        public static void RegisterFunc<T1, T2, T3, T4, T5, T6>(string scope, Func<T1, T2, T3, T4, T5, T6> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2, T3, T4, T5, T6>(scope, func);
        }
        public static void RegisterFunc<T1, T2, T3, T4, T5, T6, T7>(string scope, Func<T1, T2, T3, T4, T5, T6, T7> func)
        {
            Accessor<ChakraContext>.Current.GlobalObject.Binding.SetFunction<T1, T2, T3, T4, T5, T6, T7>(scope, func);
        }


        public static void Invoke<T1>(string scope, T1 args)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1>(scope,args);
        }
        public static void Invoke<T1,T2>(string scope, T1 args,T2 args2)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1,T2>(scope, args,args2);
        }
        public static void Invoke<T1, T2, T3>(string scope, T1 args, T2 args2, T3 args3)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1, T2, T3>(scope, args, args2,args3);
        }
        public static void Invoke<T1, T2, T3, T4>(string scope, T1 args, T2 args2, T3 args3,T4 args4)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1, T2, T3, T4>(scope, args, args2, args3,args4);
        }
        public static void Invoke<T1, T2, T3, T4,T5>(string scope, T1 args, T2 args2, T3 args3, T4 args4, T5 args5)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1, T2, T3, T4,T5>(scope, args, args2, args3, args4, args5);
        }
        public static void Invoke<T1, T2, T3, T4, T5,T6>(string scope, T1 args, T2 args2, T3 args3, T4 args4, T5 args5, T6 args6)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1, T2, T3, T4, T5,T6>(scope, args, args2, args3, args4, args5,args6);
        }
        public static void Invoke<T1, T2, T3, T4, T5, T6, T7>(string scope, T1 args, T2 args2, T3 args3, T4 args4, T5 args5, T6 args6, T7 args7)
        {
            Accessor<ChakraContext>.Current.GlobalObject.CallMethod<T1, T2, T3, T4, T5, T6,T7>(scope, args, args2, args3, args4, args5, args6, args7);
        }
    }
}
