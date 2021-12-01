using System;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal class FD3DMemoryReadbackFactory : FRHIMemoryReadbackFactory
    {
        public override bool IsReady => m_Fence.IsCompleted;

        private FRHIFence m_Fence;

        internal FD3DMemoryReadbackFactory(FRHIDevice device) : base(device)
        {
            m_Fence = new FD3DFence(device);
        }

        protected override void RequestAsyncReadback(FRHIBuffer buffer, Action<FRHIAsyncReadbackRequest> callback) 
        {
            FAsyncReadbackRequestInfo requestInfo;
            requestInfo.target = buffer;
            requestInfo.callbackFunc = callback;
            requestInfo.resourceType = EResourceType.Buffer;
            requestInfos.Add(requestInfo);
        }

        protected override void RequestAsyncReadback(FRHIBuffer buffer, in int size, in int offset, Action<FRHIAsyncReadbackRequest> callback)
        {

        }

        protected override void RequestAsyncReadback(FRHITexture texture, Action<FRHIAsyncReadbackRequest> callback)
        {

        }

        protected override void RequestAsyncReadback(FRHITexture texture, in int mipIndex, Action<FRHIAsyncReadbackRequest> callback)
        {

        }

        protected override void RequestAsyncReadback(FRHITexture texture, in int mipIndex, in int x, in int width, in int y, in int height, in int z, in int depth, Action<FRHIAsyncReadbackRequest> callback)
        {

        }

        protected override void Release()
        {
            m_Fence?.Dispose();
        }
    }
}
