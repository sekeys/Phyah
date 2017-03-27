using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Observer
{
    public class Observer : IObserver,IDisposable
    {
        public Observer(ImmutableList<IObserver> observers)
        {
            _observers = observers;
        }
        public Observer(IEnumerable<IObserver> observers)
        {
            _observers = new ImmutableList<IObserver>(observers);
        }
        private readonly ImmutableList<IObserver> _observers;
        public  void Completed()
        {
            foreach(var item in _observers.Data)
            {
                item.Completed();
            }
        }

        public  void OnError(Exception ex)
        {
            foreach(var item in _observers.Data)
            {
                item.OnError(ex);
            }
        }

        public  void Start(Input input)
        {
            foreach(var item in _observers.Data)
            {
                item.Start(input);
            }
        }

        public IObserver Add(IObserver observer)
        {
            return new Observer(_observers.Add(observer));
        }
        public IObserver Remove(IObserver observer)
        {
            var i = Array.IndexOf(_observers.Data, observer);
            if (i < 0)
                return this;

            if (_observers.Data.Length == 2)
            {
                return _observers.Data[1 - i];
            }
            else
            {
                return new Observer(_observers.Remove(observer));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GC.SuppressFinalize(this);
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~Observer()
        {
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
