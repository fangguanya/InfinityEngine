using System;
using System.Runtime.Serialization;

namespace InfinityEngine.Core.Object
{
    public abstract class UObject : IDisposable
    {
        private bool bDisposed = false;

        public UObject()
        {
            
        }

        ~UObject()
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
