using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : FDisposable
    {
        public string name;
        internal ulong fenceValue;
        internal ID3D12Fence d3dFence;

        internal FRHIFence(FRHIDevice device, string name = null) : base()
        {
            this.name = name;
            this.d3dFence = device.d3dDevice.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        public void Signal(ID3D12CommandQueue d3d12CmdQueue)
        {
            ++fenceValue;
            d3d12CmdQueue.Signal(d3dFence, fenceValue);
        }

        public bool Completed()
        {
            if (d3dFence.CompletedValue < fenceValue)
            {
                return false;
            }
            return true;
        }

        public void WaitOnCPU(AutoResetEvent fenceEvent)
        {
            if (!Completed())
            {
                d3dFence.SetEventOnCompletion(fenceValue, fenceEvent);
                fenceEvent.WaitOne();
            }
        }

        public void WaitOnGPU(ID3D12CommandQueue d3d12CmdQueue)
        {
            d3d12CmdQueue.Wait(d3dFence, fenceValue);
        }

        protected override void Release()
        {
            d3dFence?.Dispose();
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
