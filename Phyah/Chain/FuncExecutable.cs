using Phyah.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Chain
{


    public abstract class InternalFuncExecutable<T> : ParameterExecutable
    {
        private ImmutableList<object> _Parameter;
        protected override ImmutableList<object> Parameter { get => _Parameter; }
        protected T Func { get; private set; }

        public InternalFuncExecutable(T Func)
        {
            this.Func = Func;
        }

        public void Parameters(params object[] parameter)
        {
            _Parameter = parameter.AsImmutableList();
        }
    }
    public class FuncExecutable<T> : InternalFuncExecutable<Func<T>>
    {
        public FuncExecutable(Func<T> func):base(func)
        {
        }
        public T Output { get; private set; }
        public override void Execute()
        {
            Output=Func.Invoke();
        }
    }
    public class FuncExecutable<T,T1> : InternalFuncExecutable<Func<T, T1>>
    {
        public FuncExecutable(Func<T, T1> func) : base(func)
        {
        }
        public T1 Output { get; private set; }
        public override void Execute()
        {
            Output = (T1)Func.DynamicInvoke(Parameter.Data);
        }
    }
    public class FuncExecutable<T, T1, T2> : InternalFuncExecutable<Func<T, T1, T2>>
    {
        public FuncExecutable(Func<T, T1, T2> func) : base(func)
        {
        }
        public T2 Output { get; private set; }
        public override void Execute()
        {
            Output = (T2)Func.DynamicInvoke(Parameter.Data);
        }
    }
    public class FuncExecutable<T, T1, T2, T3> : InternalFuncExecutable<Func<T, T1, T2, T3>>
    {
        public FuncExecutable(Func<T, T1, T2, T3> func) : base(func)
        {
        }
        public T3 Output { get; private set; }
        public override void Execute()
        {
            Output = (T3)Func.DynamicInvoke(Parameter.Data);
        }
    }
    public class FuncExecutable<T, T1, T2, T3, T4> : InternalFuncExecutable<Func<T, T1, T2, T3, T4>>
    {
        public FuncExecutable(Func<T, T1, T2, T3, T4> func) : base(func)
        {
        }
        public T4 Output { get; private set; }
        public override void Execute()
        {
            Output = (T4)Func.DynamicInvoke(Parameter.Data);
        }
    }
    public class FuncExecutable<T, T1, T2, T3, T4, T5> : InternalFuncExecutable<Func<T, T1, T2, T3, T4, T5>>
    {
        public FuncExecutable(Func<T, T1, T2, T3, T4, T5> func) : base(func)
        {
        }
        public T5 Output { get; private set; }
        public override void Execute()
        {
            Output = (T5)Func.DynamicInvoke(Parameter.Data);
        }
    }
    public class FuncExecutable<T, T1, T2, T3, T4, T5, T6> : InternalFuncExecutable<Func<T, T1, T2, T3, T4, T5, T6>>
    {
        public FuncExecutable(Func<T, T1, T2, T3, T4, T5, T6> func) : base(func)
        {
        }
        public T6 Output { get; private set; }
        public override void Execute()
        {
            Output = (T6)Func.DynamicInvoke(Parameter.Data);
        }
    }
}
