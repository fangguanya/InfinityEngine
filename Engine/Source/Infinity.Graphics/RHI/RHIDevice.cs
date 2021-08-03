using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDevice : FDisposable
    {
        internal ID3D12Device6 d3dDevice;
        internal IDXGIAdapter1 d3dAdapter;
        internal IDXGIFactory7 d3dFactory;

        public FRHIDevice() : base()
        {
            DXGI.CreateDXGIFactory2<IDXGIFactory7>(true, out d3dFactory);
            d3dFactory.EnumAdapters1(0, out d3dAdapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(d3dAdapter, FeatureLevel.Level_12_1, out d3dDevice);
            d3dDevice.QueryInterface<ID3D12Device6>();
        }

        protected override void Disposed()
        {
            d3dDevice?.Release();
            d3dAdapter?.Release();
            d3dFactory?.Release();
        }

        public static implicit operator ID3D12Device6(FRHIDevice device) { return device.d3dDevice; }
    }
}
