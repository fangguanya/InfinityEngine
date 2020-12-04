using Vortice.DXGI;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.UObject;

namespace InfinityEngine.Graphics.RHI
{
    internal class RHIDevice : UObject
    {
        internal ID3D12Device6 NativeDevice;
        internal IDXGIFactory7 NativeFactory;

        public RHIDevice() : base()
        {
            DXGI.CreateDXGIFactory1<IDXGIFactory7>(out NativeFactory);

            IDXGIAdapter1 NativeAdapter;
            NativeFactory.EnumAdapters1(0, out NativeAdapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(NativeAdapter, FeatureLevel.Level_12_1, out NativeDevice);
            NativeDevice.QueryInterface<ID3D12Device6>();
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            NativeDevice?.Dispose();
        }
    }
}
