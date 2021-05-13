using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDevice : FDisposer
    {
        internal ID3D12Device6 d3D12Device;
        internal IDXGIAdapter1 d3D12Adapter;
        internal IDXGIFactory7 d3D12Factory;

        public FRHIDevice() : base()
        {
            DXGI.CreateDXGIFactory2<IDXGIFactory7>(true, out d3D12Factory);
            d3D12Factory.EnumAdapters1(0, out d3D12Adapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(d3D12Adapter, FeatureLevel.Level_12_1, out d3D12Device);
            d3D12Device.QueryInterface<ID3D12Device6>();
        }

        protected override void Disposed()
        {
            d3D12Device?.Dispose();
            d3D12Adapter?.Dispose();
            d3D12Factory?.Dispose();
        }

        public static implicit operator ID3D12Device6(FRHIDevice RHIDevice) { return RHIDevice.d3D12Device; }
    }
}
