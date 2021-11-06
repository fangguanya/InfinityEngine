using System.Threading;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : FDisposable
    {
        public string name;
        public virtual bool IsCompleted => false;

        internal FRHIFence(FRHIDevice device, string name = null) { }

        internal virtual void Signal(FRHICommandContext cmdContext) { }
        internal virtual void WaitOnCPU(AutoResetEvent fenceEvent) { }
        internal virtual void WaitOnGPU(FRHICommandContext cmdContext) { }
    }

    internal class FRHIFencePool : FDisposable
    {
        bool m_CollectionCheck;
        Stack<FRHIFence> m_Stack;
        FRHIGraphicsContext m_GraphicsContext;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public FRHIFencePool(FRHIGraphicsContext graphicsContext, bool collectionCheck = true)
        {
            m_CollectionCheck = true;
            m_Stack = new Stack<FRHIFence>();
            m_GraphicsContext = graphicsContext;
            m_CollectionCheck = collectionCheck;
        }

        public FRHIFence GetTemporary(string name)
        {
            FRHIFence gpuFence;
            if (m_Stack.Count == 0)
            {
                gpuFence = m_GraphicsContext.CreateFence();
                countAll++;
            }
            else
            {
                gpuFence = m_Stack.Pop();
            }
            gpuFence.name = name;
            return gpuFence;
        }

        public void ReleaseTemporary(FRHIFence gpuFence)
        {
            m_Stack.Push(gpuFence);
        }

        protected override void Release()
        {
            m_GraphicsContext = null;
            foreach (FRHIFence gpuFence in m_Stack)
            {
                gpuFence.Dispose();
            }
        }
    }
}
