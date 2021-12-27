using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDescriptorHeapFactory : FDisposal
    {
        /*protected int descriptorSize;
        protected ID3D12DescriptorHeap nativeCPUDescriptorHeap;
        protected ID3D12DescriptorHeap nativeGPUDescriptorHeap;*/

        internal FRHIDescriptorHeapFactory(FRHIDevice device, in DescriptorHeapType descriptorType, in int descriptorCount) : base()
        {
            /*this.descriptorSize = device.nativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescriptor descriptorCPU = new DescriptorHeapDescriptor(descriptorType, descriptorCount, DescriptorHeapFlags.ShaderVisible);
            this.nativeCPUDescriptorHeap = device.nativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(descriptorCPU);

            DescriptorHeapDescriptor descriptorGPU = new DescriptorHeapDescriptor(descriptorType, descriptorCount, DescriptorHeapFlags.None);
            this.nativeGPUDescriptorHeap = device.nativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(descriptorGPU);*/
        }

        protected static DescriptorHeapType GetDescriptorType(in EDescriptorType DescriptorType)
        {
            /*DescriptorHeapType OutType = DescriptorHeapType.Sampler;

            switch (DescriptorType)
            {
                case EDescriptorType.DSV:
                    OutType = DescriptorHeapType.DepthStencilView;
                    break;

                case EDescriptorType.RTV:
                    OutType = DescriptorHeapType.RenderTargetView;
                    break;

                case EDescriptorType.CbvSrvUav:
                    OutType = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView;
                    break;
            }

            return OutType;*/
            return default;
        }

        internal int Allocator(in int count)
        {
            return 1;
        }

        internal int GetDescriptorSize()
        {
            return 1;
            //return descriptorSize;
        }

        internal CpuDescriptorHandle GetCPUHandleStart()
        {
            return default;
            //return nativeCPUDescriptorHeap.GetCPUDescriptorHandleForHeapStart();
        }

        internal GpuDescriptorHandle GetGPUHandleStart()
        {
            return default;
            //return nativeGPUDescriptorHeap.GetGPUDescriptorHandleForHeapStart();
        }

        protected override void Release()
        {
            //nativeCPUDescriptorHeap?.Dispose();
            //nativeGPUDescriptorHeap?.Dispose();
        }
    }
}
