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
    public delegate void FGraphicsTask(FRenderContext renderContext);

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
        private FRHIContext m_Context;
        private FRHISwapChain m_SwapChain;
        private FRenderContext m_RenderContext;
        private FRenderPipeline m_RenderPipeline;

        public FGraphicsSystem(FWindow window, FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            IsLoopExit = false;
            m_SemaphoreG2R = semaphoreG2R;
            m_SemaphoreR2G = semaphoreR2G;
            m_RenderThread = new Thread(GraphicsFunc);
            m_RenderThread.Name = "m_RenderThread";
            m_Context = new FD3DContext();
            m_RenderContext = new FRenderContext(m_Context);
            m_RenderPipeline = new FUniversalRenderPipeline("UniversalRP");
            m_SwapChain = m_Context.CreateSwapChain("SwapChain", (uint)window.width, (uint)window.height, window.handle);
            m_SwapChain.InitResourceView(m_Context);
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
                    m_RenderPipeline.Init(m_RenderContext); 
                }
                m_RenderPipeline.Render(m_RenderContext, m_SwapChain.backBufferView);
                FRHIContext.SubmitAndFlushContext(m_Context);
                m_SwapChain.Present();
                m_SemaphoreR2G.Signal();
            }
        }

        public void ProcessGraphicsTasks()
        {
            if (FGraphics.GraphicsTasks.length == 0) { return; }

            for (int i = 0; i < FGraphics.GraphicsTasks.length; ++i) {
                FGraphics.GraphicsTasks[i](m_RenderContext);
                //FGraphics.GraphicsTasks[i] = null;
            }
            FGraphics.GraphicsTasks.Clear();
        }

        protected override void Release()
        {
            ProcessGraphicsTasks();
            m_RenderPipeline?.Release(m_RenderContext);

            m_SwapChain?.Dispose();
            m_RenderContext?.Dispose();
            m_Context?.Dispose();
            m_RenderPipeline?.Dispose();
        }
    }
}
