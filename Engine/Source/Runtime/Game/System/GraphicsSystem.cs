using System.Threading;
using InfinityEngine.Game.Window;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Thread.Sync;
using InfinityEngine.Graphics.RHI.D3D;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRHIDeviceContext deviceContext, FRenderContext renderContext);

    public static class FGraphics
    {
        internal static TArray<FGraphicsTask> GraphicsTasks = new TArray<FGraphicsTask>(64);

        public static void AddTask(FGraphicsTask graphicsTask, in bool bParallel = false)
        {
            GraphicsTasks.Add(graphicsTask);
        }
    }

    internal class FGraphicsSystem : FDisposal
    {
        private bool IsLoopExit;
        private Thread m_RenderThread;
        private FSemaphore m_SemaphoreG2R;
        private FSemaphore m_SemaphoreR2G;
        private FRHISwapChain m_SwapChain;
        private FRenderContext m_RenderContext;
        private FRenderPipeline m_RenderPipeline;
        private FRHIDeviceContext m_DeviceContext;

        public FGraphicsSystem(FWindow window, FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            IsLoopExit = false;
            m_SemaphoreG2R = semaphoreG2R;
            m_SemaphoreR2G = semaphoreR2G;
            m_RenderThread = new Thread(GraphicsFunc);
            m_RenderThread.Name = "m_RenderThread";
            m_DeviceContext = new FD3DDeviceContext();
            m_RenderContext = new FRenderContext(m_DeviceContext);
            m_RenderPipeline = new FUniversalRenderPipeline("UniversalRP");
            m_SwapChain = m_DeviceContext.CreateSwapChain("SwapChain", (uint)window.width, (uint)window.height, window.handle);
        }

        public void Start()
        {
            m_RenderThread.Start();
        }

        public void Exit()
        {
            IsLoopExit = true;
            m_SemaphoreG2R.Signal();
            m_RenderThread.Join();
        }

        public void GraphicsFunc()
        {
            bool isInit = true;

            while (!IsLoopExit)
            {
                m_SemaphoreG2R.Wait();
                ProcessGraphicsTasks();
                if (isInit) {
                    isInit = false;
                    m_RenderPipeline.Init(m_DeviceContext, m_RenderContext); 
                }
                m_RenderPipeline.Render(m_DeviceContext, m_RenderContext);
                FRHIDeviceContext.SubmitAndFlushContext(m_DeviceContext);
                m_SwapChain.Present();
                m_SemaphoreR2G.Signal();
            }
        }

        public void ProcessGraphicsTasks()
        {
            if (FGraphics.GraphicsTasks.length == 0) { return; }

            for (int i = 0; i < FGraphics.GraphicsTasks.length; ++i) {
                FGraphics.GraphicsTasks[i](m_DeviceContext, m_RenderContext);
                //FGraphics.GraphicsTasks[i] = null;
            }
            FGraphics.GraphicsTasks.Clear();
        }

        protected override void Release()
        {
            ProcessGraphicsTasks();
            m_RenderPipeline?.Release(m_DeviceContext, m_RenderContext);

            m_SwapChain?.Dispose();
            m_RenderContext?.Dispose();
            m_DeviceContext?.Dispose();
            m_RenderPipeline?.Dispose();
        }
    }
}
