using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public unsafe abstract class FRHISwapChain : FDisposal
    {
        public string name;
        public virtual int backBufferIndex => 0;


        internal FRHITexture[] backBuffer;

        internal FRHISwapChain(FRHIDevice device, FRHICommandContext cmdContext, in void* windowPtr, in uint width, in uint height, string name)
        {
            this.name = name;
            this.backBuffer = new FRHITexture[2];
        }

        public abstract void Present();
    }
}