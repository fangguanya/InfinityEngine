using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DSwapChain : FRHISwapChain
    {
        public override int backBufferIndex => (int)nativeSwapChain->GetCurrentBackBufferIndex();

        internal IDXGISwapChain4* nativeSwapChain;

        internal FD3DSwapChain(FRHIDevice device, FRHICommandContext cmdContext, in void* windowPtr, in uint width, in uint height, string name) : base(device, cmdContext, windowPtr, width, height, name)
        {
            backBuffer[0] = new FD3DTexture();
            backBuffer[1] = new FD3DTexture();

            FD3DDevice d3dDevice = (FD3DDevice)device;
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;

            DXGI_MODE_DESC bufferDesc;
            bufferDesc.Width = width;
            bufferDesc.Height = height;
            bufferDesc.RefreshRate = new DXGI_RATIONAL(60, 1);
            bufferDesc.Format = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;
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

            IDXGISwapChain* swapChainPtr;
            d3dDevice.nativeFactory->CreateSwapChain((IUnknown*)d3dCmdContext.nativeCmdQueue, &swapChainDesc, &swapChainPtr);
            nativeSwapChain = (IDXGISwapChain4*)swapChainPtr;

            ID3D12Resource* backbufferResourceA;
            nativeSwapChain->GetBuffer(0, Windows.__uuidof<ID3D12Resource>(), (void**)&backbufferResourceA);
            fixed (char* namePtr = name + "_BackBuffer0")
            {
                backbufferResourceA->SetName((ushort*)namePtr);
            }
            ((FD3DTexture)backBuffer[0]).defaultResource = backbufferResourceA;

            ID3D12Resource* backbufferResourceB;
            nativeSwapChain->GetBuffer(1, Windows.__uuidof<ID3D12Resource>(), (void**)&backbufferResourceB);
            fixed (char* namePtr = name + "_BackBuffer1")
            {
                backbufferResourceB->SetName((ushort*)namePtr);
            }
            ((FD3DTexture)backBuffer[1]).defaultResource = backbufferResourceB;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Present()
        {
            nativeSwapChain->Present(1, 0);
        }

        protected override void Release()
        {
            nativeSwapChain->Release();
        }
    }
}