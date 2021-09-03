using System;
using System.Runtime.Serialization;

namespace InfinityEngine.Core.Object
{
    public class FDisposable : IDisposable
    {
        private bool bDisposed = false;

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
            if (!bDisposed)
            {
                Release();
            }

            bDisposed = true;
        }

        public void Dispose()
        {
            Finalizer();
            GC.SuppressFinalize(this);
        }
    }
}
