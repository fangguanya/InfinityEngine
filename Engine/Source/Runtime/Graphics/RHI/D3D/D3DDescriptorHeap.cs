using System;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Container;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal static class FD3DDescriptorUtil
    {
        public static D3D12_DESCRIPTOR_HEAP_TYPE GetDescriptorType(in EDescriptorType type)
        {
            switch (type)
            {
                case EDescriptorType.DSV:
                    return D3D12_DESCRIPTOR_HEAP_TYPE.D3D12_DESCRIPTOR_HEAP_TYPE_DSV;

                case EDescriptorType.RTV:
                    return D3D12_DESCRIPTOR_HEAP_TYPE.D3D12_DESCRIPTOR_HEAP_TYPE_RTV;

                case EDescriptorType.CbvSrvUav:
                    return D3D12_DESCRIPTOR_HEAP_TYPE.D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV;
            }

            return D3D12_DESCRIPTOR_HEAP_TYPE.D3D12_DESCRIPTOR_HEAP_TYPE_SAMPLER;
        }
    }

    internal unsafe class FD3DDescriptorHeapFactory : FRHIDescriptorHeapFactory
    {
        public uint descriptorSize => m_DescriptorSize;
        public D3D12_CPU_DESCRIPTOR_HANDLE cpuStartHandle => m_CPUDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
        public D3D12_GPU_DESCRIPTOR_HANDLE gpuStartHandle => m_GPUDescriptorHeap->GetGPUDescriptorHandleForHeapStart();

        private uint m_DescriptorSize;
        private TValueArray<int> m_CacheMap;
        private ID3D12DescriptorHeap* m_CPUDescriptorHeap;
        private ID3D12DescriptorHeap* m_GPUDescriptorHeap;

        public FD3DDescriptorHeapFactory(FRHIDevice device, in EDescriptorType type, in uint count, string name) : base(device, type, count, name)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;
            D3D12_DESCRIPTOR_HEAP_TYPE heapType = FD3DDescriptorUtil.GetDescriptorType(type);
            m_DescriptorSize = d3dDevice.nativeDevice->GetDescriptorHandleIncrementSize(heapType);

            m_CacheMap = new TValueArray<int>((int)count);
            for(int i = 0; i < (int)count; ++i)
            {
                m_CacheMap.Add(i);
            }

            D3D12_DESCRIPTOR_HEAP_DESC descriptorCPU;
            descriptorCPU.Flags = D3D12_DESCRIPTOR_HEAP_FLAGS.D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
            descriptorCPU.Type = heapType;
            descriptorCPU.NumDescriptors = count;
            ID3D12DescriptorHeap* cpuHeapPtr;
            d3dDevice.nativeDevice->CreateDescriptorHeap(&descriptorCPU, Windows.__uuidof<ID3D12DescriptorHeap>(), (void**)&cpuHeapPtr);
            fixed (char* namePtr = name + "_CPU")
            {
                cpuHeapPtr->SetName((ushort*)namePtr);
            }
            m_CPUDescriptorHeap = cpuHeapPtr;

            if(type != EDescriptorType.CbvSrvUav) { return; }
            D3D12_DESCRIPTOR_HEAP_DESC descriptorGPU;
            descriptorCPU.Flags = D3D12_DESCRIPTOR_HEAP_FLAGS.D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
            descriptorCPU.Type = heapType;
            descriptorCPU.NumDescriptors = count;
            ID3D12DescriptorHeap* gpuHeapPtr;
            d3dDevice.nativeDevice->CreateDescriptorHeap(&descriptorGPU, Windows.__uuidof<ID3D12DescriptorHeap>(), (void**)&gpuHeapPtr);
            fixed (char* namePtr = name + "_GPU")
            {
                gpuHeapPtr->SetName((ushort*)namePtr);
            }
            m_GPUDescriptorHeap = gpuHeapPtr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Allocate()
        {
            int index = m_CacheMap[m_CacheMap.length - 1];
            m_CacheMap.RemoveSwapAtIndex(m_CacheMap.length - 1);
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Allocate(in int count)
        {
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Free(in int index)
        {
            m_CacheMap.Add(index);
        }

        protected override void Release()
        {
            m_CacheMap.Dispose();
            m_CPUDescriptorHeap->Release();
            if (m_Type != EDescriptorType.CbvSrvUav) { return; }
            m_GPUDescriptorHeap->Release();
        }
    }
}
