using System;
using System.Threading;
using System.Runtime.Versioning;
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
    public delegate void FGraphicsTask(FRenderContext m_RenderContext, FRHIGraphicsContext m_GraphicsContext);

    public static class FGraphics
    {
        internal static TArray<FGraphicsTask> GraphicsTasks = new TArray<FGraphicsTask>(64);

        public static void AddTask(FGraphicsTask graphicsTask, in bool bParallel = false)
        {
            GraphicsTasks.Add(graphicsTask);
        }
    }

    [SupportedOSPlatform("windows10.0.19042")]
    internal class FGraphicsSystem : FDisposal
    {
        private bool IsLoopExit;
        private Thread m_RenderThread;
        private FSemaphore m_SemaphoreG2R;
        private FSemaphore m_SemaphoreR2G;
        private FRHISwapChain m_SwapChain;
        private FRenderContext m_RenderContext;
        private FRenderPipeline m_RenderPipeline;
        private FRHIGraphicsContext m_GraphicsContext;

        public FGraphicsSystem(FWindow window, FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            IsLoopExit = false;
            m_SemaphoreG2R = semaphoreG2R;
            m_SemaphoreR2G = semaphoreR2G;
            m_RenderThread = new Thread(GraphicsFunc);
            m_RenderThread.Name = "m_RenderThread";

            m_RenderContext = new FRenderContext();
            m_GraphicsContext = new FD3DGraphicsContext();
            m_RenderPipeline = new FUniversalRenderPipeline("UniversalRP");

            m_SwapChain = m_GraphicsContext.CreateSwapChain((uint)window.width, (uint)window.height, window.handle);
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
                    m_RenderPipeline.Init(m_RenderContext, m_GraphicsContext); 
                }
                m_RenderPipeline.Render(m_RenderContext, m_GraphicsContext);
                FRHIGraphicsContext.SubmitAndFlushContext(m_GraphicsContext);
                m_SwapChain.Present();
                m_SemaphoreR2G.Signal();
            }
        }

        public void ProcessGraphicsTasks()
        {
            if (FGraphics.GraphicsTasks.length == 0) { return; }

            for (int i = 0; i < FGraphics.GraphicsTasks.length; ++i) {
                FGraphics.GraphicsTasks[i](m_RenderContext, m_GraphicsContext);
                //FGraphics.GraphicsTasks[i] = null;
            }
            FGraphics.GraphicsTasks.Clear();
        }

        protected override void Release()
        {
            ProcessGraphicsTasks();
            m_RenderPipeline?.Release(m_RenderContext, m_GraphicsContext);

            m_SwapChain?.Dispose();
            m_RenderContext?.Dispose();
            m_RenderPipeline?.Dispose();
            m_GraphicsContext?.Dispose();
        }
    }
}
