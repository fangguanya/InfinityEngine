using System;
using System.Runtime.Serialization;

namespace InfinityEngine.Core.Object
{
    public class FDisposable : IDisposable
    {
        private bool IsDisposed = false;

        public FDisposable()
        {
            
        }

        ~FDisposable()
        {
            Finalizer();
        }

        protected virtual void Release() { }

        private void Finalizer()
        {
            if (!IsDisposed)
            {
                Release();
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Finalizer();
            GC.SuppressFinalize(this);
        }
    }
}
