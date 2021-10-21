using System.Threading;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : FDisposable
    {
        public string name;
        public bool IsCompleted
        {
            get { return m_NativeFence.CompletedValue >= m_FenceValue ? true : false; }
        }
        private ulong m_FenceValue;
        private ID3D12Fence m_NativeFence;

        internal FRHIFence(FRHIDevice device, string name = null) : base()
        {
            this.name = name;
            this.m_NativeFence = device.nativeDevice.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        internal void Signal(FRHICommandContext cmdContext)
        {
            ++m_FenceValue;
            cmdContext.nativeCmdQueue.Signal(m_NativeFence, m_FenceValue);
        }

        internal void WaitOnCPU(AutoResetEvent fenceEvent)
        {
            if (!IsCompleted)
            {
                m_NativeFence.SetEventOnCompletion(m_FenceValue, fenceEvent);
                fenceEvent.WaitOne();
            }
        }

        internal void WaitOnGPU(FRHICommandContext cmdContext)
        {
            cmdContext.nativeCmdQueue.Wait(m_NativeFence, m_FenceValue);
        }

        protected override void Release()
        {
            m_NativeFence?.Dispose();
        }
    }

    internal class FRHIFencePool : FDisposable
    {
        private FRHIDevice m_Device;
        readonly bool m_CollectionCheck = true;
        readonly Stack<FRHIFence> m_Stack = new Stack<FRHIFence>();
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        internal FRHIFencePool(FRHIDevice device, bool collectionCheck = true)
        {
            m_Device = device;
            m_CollectionCheck = collectionCheck;
        }

        public FRHIFence GetTemporary(string name)
        {
            FRHIFence element;
            if (m_Stack.Count == 0)
            {
                element = new FRHIFence(m_Device);
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            element.name = name;
            return element;
        }

        public void ReleaseTemporary(FRHIFence element)
        {
#if WITH_EDITOR // keep heavy checks in editor
            if (m_CollectionCheck && m_Stack.Count > 0)
            {
                if (m_Stack.Contains(element))
                    Console.WriteLine("Internal error. Trying to destroy object that is already released to pool.");
            }
#endif
            m_Stack.Push(element);
        }

        protected override void Release()
        {
            m_Device = null;
            foreach (FRHIFence cmdList in m_Stack)
            {
                cmdList.Dispose();
            }
        }
    }
}
