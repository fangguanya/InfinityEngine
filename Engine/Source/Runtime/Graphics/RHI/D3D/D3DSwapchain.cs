using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using System.Runtime.Versioning;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DSwapChain : FRHISwapChain
    {
        internal IDXGISwapChain* nativeSwapChain;

        [SupportedOSPlatform("windows10.0.19042")]
        internal FD3DSwapChain(FRHIDevice device, FRHICommandContext cmdContext, in void* windowPtr, in uint width, in uint height) : base(device, cmdContext, windowPtr, width, height)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;


            DXGI_MODE_DESC bufferDesc;
            bufferDesc.Width = width;
            bufferDesc.Height = height;
            bufferDesc.RefreshRate = new DXGI_RATIONAL(60, 1);
            bufferDesc.Format = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_TYPELESS;
            bufferDesc.Scaling = DXGI_MODE_SCALING.DXGI_MODE_SCALING_UNSPECIFIED;
            bufferDesc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER.DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED;

            DXGI_SAMPLE_DESC sampleDesc;
            sampleDesc.Count = 1;
            sampleDesc.Quality = 0;

            DXGI_SWAP_CHAIN_DESC swapChainDesc;
            swapChainDesc.Windowed = true;
            swapChainDesc.BufferCount = 2;
            swapChainDesc.BufferDesc = bufferDesc;
            swapChainDesc.SampleDesc = sampleDesc;
            swapChainDesc.OutputWindow = new HWND(windowPtr);
            swapChainDesc.BufferUsage = DXGI.DXGI_USAGE_RENDER_TARGET_OUTPUT;
            swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_DISCARD;

            IDXGISwapChain* swapChain = null;
            d3dDevice.nativeFactory->CreateSwapChain((IUnknown*)d3dCmdContext.nativeCmdQueue, &swapChainDesc, &swapChain);
            nativeSwapChain = swapChain;
        }
    }
}