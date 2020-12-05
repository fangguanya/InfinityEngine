using System;

namespace InfinityEngine.Core.Object
{
    public abstract class FObject : IDisposable
    {
        private bool IsDisposed = false;

        public FObject()
        {
            
        }

        ~FObject()
        {
            Dispose(false);
        }

        protected abstract void DisposeManaged();

        protected abstract void DisposeUnManaged();

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    DisposeManaged();
                }
                DisposeUnManaged();
            }
            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
