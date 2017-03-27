

namespace Phyah
{
    using Phyah.Enumerable;
    using Phyah.Identity;
    using Phyah.Interface;
    using Phyah.Thread;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    public  class Input : IParameter
    {
        public static Input Current
        {
            get => Accessor<Input>.Current;
        }
        public Input()
        {
            Body = new Parameter();
            Accessor<Input>.Current = this;
        }

        public IUser User { get; set; }
        public object this[string field] { get => Body[field]; set => Body[field] = value; }

        public int Count => Body.Count;

        public dynamic Body { get; protected set; }
        public bool Contains(string field) => Body.Contains(field);

        public void From(Action action)
        {
            Require.NotNull(action);
            action();
        }
        public void From(Func<Tuple<string, object>> func)
        {
            var tuple = func();
            this.Set(tuple.Item1, tuple.Item2);
        }

        public string String(string field) => Get<string>(field);
        public int Int(string field) => Get<int>(field);
        public float Float(string field) => Get<float>(field);
        public bool Boolean(string field) => Get<bool>(field);
        public double Double(string field) => Get<double>(field);
        public DateTime DateTime(string field) => Get<DateTime>(field);
        public TimeSpan TimeSpan(string field) => Get<TimeSpan>(field);
        public dynamic Dynamic(string field) => Get<dynamic>(field);
        public IParameter Paramter(string field) => Get<IParameter>(field);

        public object Get(string field)
        {
            return Body.Get(field);
        }

        public T Get<T>(string field) => Body.Get<T>(field);

        public T Get<T>(string field, T defValue) => Body.Get<T>(field, defValue);

        public bool Is<T>(string field) => Body.Is<T>(field);

        public void Set(string field, object value) => Body.Set(field, value);

        public  void FromStream(Stream stream)
        {
            Stream = stream;
        }
        public void FromStream(Stream stream,Action<Input> action)
        {
            Stream = stream;
            action(this);
        }
        public Stream Stream { get; protected set; }

        public Protocols Protocol { get; set; }

        public Verbs Verb { get; set; }
    }
}
