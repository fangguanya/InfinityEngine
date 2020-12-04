using System;

namespace InfinityEngine.Core.UObject
{
    public abstract class UObject : IDisposable
    {
        private bool IsDisposed = false;

        public UObject()
        {
            
        }

        ~UObject()
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
