using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D12;

namespace InfinityEngine.Graphics.RHI
{
    internal class FD3DDevice : FRHIDevice
    {
        internal ID3D12Device6 nativeDevice;
        internal IDXGIAdapter1 nativeAdapter;
        internal IDXGIFactory7 nativeFactory;

        public FD3DDevice()
        {
            DXGI.CreateDXGIFactory2<IDXGIFactory7>(true, out nativeFactory);
            nativeFactory.EnumAdapters1(0, out nativeAdapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(nativeAdapter, FeatureLevel.Level_12_1, out nativeDevice);
            nativeDevice.QueryInterface<ID3D12Device6>();
        }

        protected override void Release()
        {
            nativeDevice?.Dispose();
            nativeAdapter?.Dispose();
            nativeFactory?.Dispose();
        }

        public static implicit operator ID3D12Device6(FD3DDevice device) { return device.nativeDevice; }
    }
}
