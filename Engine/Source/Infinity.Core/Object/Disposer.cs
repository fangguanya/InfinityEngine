using System;
using System.Runtime.Serialization;

namespace InfinityEngine.Core.Object
{
    public class FDisposer : IDisposable
    {
        private bool m_IsDisposed = false;

        public FDisposer()
        {
            
        }

        ~FDisposer()
        {
            Finalizer();
        }

        protected virtual void Disposed() { }

        private void Finalizer()
        {
            if (!m_IsDisposed)
            {
                Disposed();
            }

            m_IsDisposed = true;
        }

        public void Dispose()
        {
            Finalizer();
            GC.SuppressFinalize(this);
        }
    }
}
