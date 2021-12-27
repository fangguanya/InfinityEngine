using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI
{
    public struct FRHIAsyncReadbackRequest
    {
        public bool IsReady => m_Fence.IsCompleted;

        private FRHIFence m_Fence;

        internal FRHIAsyncReadbackRequest(FRHIFence fence)
        {
            m_Fence = fence;
        }

        public void GetData(ref IntPtr data)
        {
            
        }

        public void GetData<T>(T[] data) where T : struct
        {

        }

        public void GetData<T>(ref Span<T> data) where T : struct
        {

        }
    }

    public struct FAsyncReadbackRequestInfo
    {
        public FRHIResource target;
        public EResourceType resourceType;
        public Action<FRHIAsyncReadbackRequest> callbackFunc;
    }

    internal class FRHIMemoryReadbackFactory : FDisposal
    {
        public virtual bool IsReady => false;
        public TArray<FAsyncReadbackRequestInfo> requestInfos;

        public FRHIMemoryReadbackFactory(FRHIDevice device) 
        {
            requestInfos = new TArray<FAsyncReadbackRequestInfo>(16);
        }
        public void Clear()
        {
            requestInfos.Clear();
        }
        protected virtual void RequestAsyncReadback(FRHIBuffer buffer, Action<FRHIAsyncReadbackRequest> callback) { }
        protected virtual void RequestAsyncReadback(FRHIBuffer buffer, in int size, in int offset, Action<FRHIAsyncReadbackRequest> callback) { }
        protected virtual void RequestAsyncReadback(FRHITexture texture, Action<FRHIAsyncReadbackRequest> callback) { }
        protected virtual void RequestAsyncReadback(FRHITexture texture, in int mipIndex, Action<FRHIAsyncReadbackRequest> callback) { }
        protected virtual void RequestAsyncReadback(FRHITexture texture, in int mipIndex, in int x, in int width, in int y, in int height, in int z, in int depth, Action<FRHIAsyncReadbackRequest> callback) { }
    }
}
