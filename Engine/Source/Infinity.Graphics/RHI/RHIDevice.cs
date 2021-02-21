using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDevice : UObject
    {
        internal ID3D12Device6 NativeDevice;
        internal IDXGIAdapter1 NativeAdapter;
        internal IDXGIFactory7 NativeFactory;

        public FRHIDevice() : base()
        {
            DXGI.CreateDXGIFactory2<IDXGIFactory7>(true, out NativeFactory);
            NativeFactory.EnumAdapters1(0, out NativeAdapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(NativeAdapter, FeatureLevel.Level_12_1, out NativeDevice);
            NativeDevice.QueryInterface<ID3D12Device6>();
        }

        protected override void Disposed()
        {
            NativeDevice?.Dispose();
            NativeAdapter?.Dispose();
            NativeFactory?.Dispose();
        }

        public static implicit operator ID3D12Device6(FRHIDevice RHIDevice) { return RHIDevice.NativeDevice; }
    }
}
