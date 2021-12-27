using System;
using System.Runtime.Serialization;

namespace InfinityEngine.Core.Object
{
    public class FDisposal : IDisposable
    {
        private bool IsDisposed = false;

        public FDisposal()
        {
            
        }

        ~FDisposal()
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
