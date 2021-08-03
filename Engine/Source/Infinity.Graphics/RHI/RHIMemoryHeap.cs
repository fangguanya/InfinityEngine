using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
{
    internal sealed class FRHIMemoryHeapFactory : FDisposable
    {
        internal FRHIMemoryHeapFactory(ID3D12Device6 d3dDevice, in int heapCount) : base()
        {

        }

        protected override void Disposed()
        {

        }
    }
}
