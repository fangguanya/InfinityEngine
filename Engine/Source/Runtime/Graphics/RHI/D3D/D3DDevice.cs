using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal unsafe class FD3DDevice : FRHIDevice
    {
        internal ID3D12Device6* nativeDevice;
        internal IDXGIAdapter1* nativeAdapter;
        internal IDXGIFactory7* nativeFactory;

        public FD3DDevice()
        {
            IDXGIFactory7* factory = null;
            DirectX.CreateDXGIFactory2(0, Windows.__uuidof<IDXGIFactory7>(), (void**)&factory);
            nativeFactory = factory;

            IDXGIAdapter1* adapter = null;
            factory->EnumAdapters1(0, &adapter);
            nativeAdapter = adapter;

            ID3D12Device6* device = null;
            DirectX.D3D12CreateDevice((IUnknown*)adapter, D3D_FEATURE_LEVEL.D3D_FEATURE_LEVEL_12_1, Windows.__uuidof<ID3D12Device6>(), (void**)&device);
            nativeDevice = device;
        }

        protected override void Release()
        {
            nativeDevice->Release();
            nativeAdapter->Release();
            nativeFactory->Release();
        }

        public static implicit operator ID3D12Device6*(FD3DDevice device) { return device.nativeDevice; }
    }
}
