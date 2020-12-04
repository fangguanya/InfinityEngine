using System;

namespace Infinity.Runtime.Graphics.Core
{
    public abstract class TObject : IDisposable
    {
        private bool IsDisposed = false;

        public TObject()
        {
            
        }

        ~TObject()
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
