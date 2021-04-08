using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityEngine.Graphics.RDG
{
    public struct FRDGPassBuilder : IDisposable
    {
        bool m_Disposed;
        IRDGPass m_RenderPass;

        void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            m_Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
