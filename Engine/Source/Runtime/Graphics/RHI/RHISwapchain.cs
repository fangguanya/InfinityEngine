using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public unsafe class FRHISwapChain : FDisposal
    {
        public string name;

        internal FRHISwapChain(FRHIDevice device, FRHICommandContext cmdContext, in void* windowPtr, in uint width, in uint height)
        {

        }
    }
}