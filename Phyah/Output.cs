using Phyah.Enumerable;
using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phyah
{
    public class Output : IParameter, IDisposable
    {
        protected Output()
        {
            Body = new Parameter();
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GC.Collect();
                }
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~Output()
        {
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public dynamic Body
        {
            get;
            protected set;
        }

        public object this[string field] { get => Body[field]; set => Body[field] = value; }

        public int Count => Body.Count;
        public int Int(string field) => Get<int>(field);
        public float Float(string field) => Get<float>(field);
        public bool Boolean(string field) => Get<bool>(field);
        public double Double(string field) => Get<double>(field);
        public DateTime DateTime(string field) => Get<DateTime>(field);
        public TimeSpan TimeSpan(string field) => Get<TimeSpan>(field);
        public dynamic Dynamic(string field) => Get<dynamic>(field);
        public object Get(string field)
        {
            return Body.Get(field);
        }

        public T Get<T>(string field) => Body.Get<T>(field);

        public T Get<T>(string field, T defValue) => Body.Get<T>(field, defValue);

        public bool Is<T>(string field) => Body.Is<T>(field);

        public void Set(string field, object value) => Body.Set(field, value);

        #region Body 注释
        //public int BodyInt(string field) => Body.Get<int>(field);
        //public float BodyFloat(string field) => Body.Get<float>(field);
        //public bool BodyBoolean(string field) => Body.Get<bool>(field);
        //public double BodyDouble(string field) => Body.Get<double>(field);
        //public DateTime BodyDateTime(string field) => Body.Get<DateTime>(field);
        //public TimeSpan BodyTimeSpan(string field) => Body.Get<TimeSpan>(field);
        //public dynamic BodyDynamic(string field) => Body.Get<dynamic>(field);

        //public object BodyGet(string field)
        //{
        //    return Body.Get(field);
        //}

        //public T BodyGet<T>(string field) => Body.Get<T>(field);

        //public T BodyGet<T>(string field, T defValue) => Body.Get<T>(field, defValue);

        //public bool BodyIs<T>(string field) => Body.Is<T>(field);

        //public void BodySet(string field, object value) => Body.Set(field, value);

        //public bool BodyContains(string field) => Body.Contains(field);
        #endregion
        public Stream Stream { get; protected set; }
        public Verbs Verb { get; set; }

        public static Output FromInput(Input input)
        {
            if (input.Stream.CanRead)
            {
                var output = new Output();
                output.Stream = input.Stream;
                return output;
            }
            return default(Output);
        }
        public static Output FromStream(Stream stream)
        {
            if (stream.CanRead)
            {
                var output = new Output();
                output.Stream = stream;
                return output;
            }
            return default(Output);
        }

        public bool Contains(string field) => Body.Contains(field);

        public int Status { get; set; } = 200;
    }
}
