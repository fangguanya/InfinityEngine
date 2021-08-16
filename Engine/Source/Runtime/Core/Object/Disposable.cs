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

        protected virtual void Disposed() { }

        private void Finalizer()
        {
            if (!bDisposed)
            {
                Disposed();
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
