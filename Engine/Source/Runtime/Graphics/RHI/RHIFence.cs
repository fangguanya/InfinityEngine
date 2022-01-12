using System.Threading;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public abstract class FRHIFence : FDisposal
    {
        public string name;
        public virtual bool IsCompleted => false;

        internal FRHIFence(FRHIDevice device, string name) { }

        internal abstract void Signal(FRHICommandContext cmdContext);
        internal abstract void WaitOnCPU(AutoResetEvent fenceEvent);
        internal abstract void WaitOnGPU(FRHICommandContext cmdContext);
    }

    internal class FRHIFencePool : FDisposal
    {
        Stack<FRHIFence> m_Pooled;
        FRHIDeviceContext m_DeviceContext;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Pooled.Count; } }

        public FRHIFencePool(FRHIDeviceContext deviceContext)
        {
            m_Pooled = new Stack<FRHIFence>();
            m_DeviceContext = deviceContext;
        }

        public FRHIFence GetTemporary(string name)
        {
            FRHIFence gpuFence;
            if (m_Pooled.Count == 0)
            {
                gpuFence = m_DeviceContext.CreateFence(name);
                countAll++;
            }
            else
            {
                gpuFence = m_Pooled.Pop();
            }
            gpuFence.name = name;
            return gpuFence;
        }

        public void ReleaseTemporary(FRHIFence gpuFence)
        {
            m_Pooled.Push(gpuFence);
        }

        protected override void Release()
        {
            m_DeviceContext = null;
            foreach (FRHIFence gpuFence in m_Pooled)
            {
                gpuFence.Dispose();
            }
        }
    }
}
