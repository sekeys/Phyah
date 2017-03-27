using Phyah.Observer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Chain
{


    public class ActionExecutable : IExecutable
    {
        private readonly Action Action;

        public ActionExecutable(Action action)
        {
            Action = action;
        }
        public void Execute()
        {
            Action();
        }
    }
    public abstract class ParameterExecutable : IExecutable
    {

        protected abstract ImmutableList<object> Parameter { get; }
        public abstract void Execute();
    }
    public class ActionExecutable<T> : ParameterExecutable
    {
        private ImmutableList<object> _Parameter;
        protected override ImmutableList<object> Parameter { get => _Parameter; }
        protected Action<T> Action { get; private set; }

        public ActionExecutable(Action<T> action)
        {
            Action = action;
        }

        public ActionExecutable(Action<T> action, params object[] parameter)
        {
            Action = action;
            Parameters(parameter);
        }
        public void Parameters(params object[] parameter)
        {
            _Parameter = parameter.AsImmutableList();
        }
        public override void Execute()
        {
            Action.Invoke((T)_Parameter.Data[0]);
        }
    }

    public abstract class InternalActionExecutable<T> : ParameterExecutable
    {
        private ImmutableList<object> _Parameter;
        protected override ImmutableList<object> Parameter { get => _Parameter; }
        protected T Action { get; private set; }

        public InternalActionExecutable(T action)
        {
            Action = action;
        }
        public InternalActionExecutable(T action, params object[] parameter)
        {
            Action = action;
            Parameters(parameter);
        }

        public void Parameters(params object[] parameter)
        {
            _Parameter = parameter.AsImmutableList();
        }
    }
    public class ActionExecutable<T, T2> : ParameterExecutable
    {
        private ImmutableList<object> _Parameter;
        protected override ImmutableList<object> Parameter { get => _Parameter; }
        protected Action<T, T2> Action { get; private set; }

        public ActionExecutable(Action<T, T2> action)
        {
            Action = action;
        }
        public ActionExecutable(Action<T, T2> action, params object[] parameter)
        {
            Action = action;
            Parameters(parameter);
        }
        public void Parameters(params object[] parameter)
        {
            _Parameter = parameter.AsImmutableList();
        }
        public override void Execute()
        {
            Action.DynamicInvoke(_Parameter.Data);
        }
    }
    public class ActionExecutable<T1, T2, T3> : InternalActionExecutable<Action<T1, T2, T3>>
    {
        public ActionExecutable(Action<T1, T2, T3> action) : base(action) { }
        public ActionExecutable(Action<T1, T2, T3> action,params object[] parameter) : base(action, parameter) { }
        public override void Execute()
        {
            Action.DynamicInvoke(Parameter.Data);
        }
    }
    public class ActionExecutable<T1, T2, T3, T4> : InternalActionExecutable<Action<T1, T2, T3, T4>>
    {
        public ActionExecutable(Action<T1, T2, T3, T4> action) : base(action) { }
        public ActionExecutable(Action<T1, T2, T3,T4> action, params object[] parameter) : base(action, parameter) { }
        public override void Execute()
        {
            Action.DynamicInvoke(Parameter.Data);
        }
    }
    public class ActionExecutable<T1, T2, T3, T4, T5> : InternalActionExecutable<Action<T1, T2, T3, T4, T5>>
    {
        public ActionExecutable(Action<T1, T2, T3,T4,T5> action, params object[] parameter) : base(action, parameter) { }
        public ActionExecutable(Action<T1, T2, T3, T4, T5> action) : base(action) { }
        public override void Execute()
        {
            Action.DynamicInvoke(Parameter.Data);
        }
    }
    public class ActionExecutable<T1, T2, T3, T4, T5, T6> : InternalActionExecutable<Action<T1, T2, T3, T4, T5, T6>>
    {
        public ActionExecutable(Action<T1, T2, T3,T4,T5,T6> action, params object[] parameter) : base(action, parameter) { }
        public ActionExecutable(Action<T1, T2, T3, T4, T5, T6> action) : base(action) { }
        public override void Execute()
        {
            Action.DynamicInvoke(Parameter.Data);
        }
    }
    public class ActionExecutable<T1, T2, T3, T4, T5, T6, T7> : InternalActionExecutable<Action<T1, T2, T3, T4, T5, T6,T7>>
    {
        public ActionExecutable(Action<T1, T2, T3, T4, T5, T6, T7> action, params object[] parameter) : base(action, parameter) { }
        public ActionExecutable(Action<T1, T2, T3, T4, T5, T6, T7> action) : base(action) { }
        public override void Execute()
        {
            Action.DynamicInvoke(Parameter.Data);
        }
    }
}
