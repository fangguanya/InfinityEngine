using System.Threading;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal unsafe class FD3DCommandContext : FRHICommandContext
    {
        private FD3DFence m_Fence;
        private ID3D12CommandQueue* m_NativeCmdQueue;
        private ID3D12CommandAllocator* m_NativeCmdAllocator;

        internal ID3D12CommandQueue* nativeCmdQueue
        {
            get
            {
                return m_NativeCmdQueue;
            }
        }
        internal ID3D12CommandAllocator* nativeCmdAllocator
        {
            get
            {
                return m_NativeCmdAllocator;
            }
        }

        internal FD3DCommandContext(FRHIDevice device, EContextType contextType, string name) : base(device, contextType, name)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;

            m_Fence = new FD3DFence(device, name);
            m_FenceEvent = new AutoResetEvent(false);

            D3D12_COMMAND_QUEUE_DESC queueDescriptor;
            queueDescriptor.Priority = 0;
            queueDescriptor.NodeMask = 0;
            queueDescriptor.Type = (D3D12_COMMAND_LIST_TYPE)contextType;
            queueDescriptor.Flags = D3D12_COMMAND_QUEUE_FLAGS.D3D12_COMMAND_QUEUE_FLAG_NONE;

            ID3D12CommandQueue* commandQueuePtr;
            d3dDevice.nativeDevice->CreateCommandQueue(&queueDescriptor, Windows.__uuidof<ID3D12CommandQueue>(), (void**)&commandQueuePtr);
            fixed (char* namePtr = name + "_Queue")
            {
                commandQueuePtr->SetName((ushort*)namePtr);
            }
            m_NativeCmdQueue = commandQueuePtr;

            ID3D12CommandAllocator* cmdAllocatorPtr;
            d3dDevice.nativeDevice->CreateCommandAllocator((D3D12_COMMAND_LIST_TYPE)contextType, Windows.__uuidof<ID3D12CommandAllocator>(), (void**)&cmdAllocatorPtr);
            fixed (char* namePtr = name + "_CmdAllocator")
            {
                cmdAllocatorPtr->SetName((ushort*)namePtr);
            }
            m_NativeCmdAllocator = cmdAllocatorPtr;
        }

        public static implicit operator ID3D12CommandQueue*(FD3DCommandContext cmdContext) { return cmdContext.m_NativeCmdQueue; }

        public override void SignalQueue(FRHIFence fence)
        {
            fence.Signal(this);
        }

        public override void WaitQueue(FRHIFence fence)
        {
            fence.WaitOnGPU(this);
        }

        public override void ExecuteQueue(FRHICommandBuffer cmdBuffer)
        {
            cmdBuffer.Close();
            FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
            ID3D12CommandList** ppCommandLists = stackalloc ID3D12CommandList*[1] { (ID3D12CommandList*)d3dCmdBuffer.nativeCmdList };
            m_NativeCmdQueue->ExecuteCommandLists(1, ppCommandLists);
        }

        public override void Flush()
        {
            m_Fence.Signal(this);
            m_Fence.WaitOnCPU(m_FenceEvent);
        }

        protected override void Release()
        {
            m_Fence?.Dispose();
            m_FenceEvent?.Dispose();
            m_NativeCmdQueue->Release();
            m_NativeCmdAllocator->Release();
        }
    }
}
