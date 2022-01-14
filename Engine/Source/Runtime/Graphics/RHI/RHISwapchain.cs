using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI
{
    public unsafe abstract class FRHISwapChain : FDisposal
    {
        public string name;
        public virtual int swapIndex => 0;
        public FRHITexture backBuffer => backBuffers[swapIndex];
        public FRHIRenderTargetView backBufferView => backBufferViews[swapIndex];

        protected FRHITexture[] backBuffers;
        protected FRHIRenderTargetView[] backBufferViews;

        internal FRHISwapChain(FRHIDevice device, FRHICommandContext cmdContext, in void* windowPtr, in uint width, in uint height, string name)
        {
            this.name = name;
            this.backBuffers = new FRHITexture[2];
            this.backBufferViews = new FRHIRenderTargetView[2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Present();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void InitResourceView(FRHIContext context);
    }
}