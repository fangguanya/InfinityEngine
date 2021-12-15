using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Thread.Sync;
using InfinityEngine.Graphics.RHI.D3D;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

    public static class FGraphics
    {
        internal static TArray<FGraphicsTask> GraphicsTasks = new TArray<FGraphicsTask>(64);

        public static void AddTask(FGraphicsTask graphicsTask, in bool bParallel = false)
        {
            GraphicsTasks.Add(graphicsTask);
        }
    }

    internal class FGraphicsSystem : FDisposable
    {
        private bool IsLoopExit;
        internal Thread renderThread;
        internal FSemaphore semaphoreG2R;
        internal FSemaphore semaphoreR2G;
        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        public FGraphicsSystem(FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            this.IsLoopExit = false;
            this.semaphoreG2R = semaphoreG2R;
            this.semaphoreR2G = semaphoreR2G;
            this.renderThread = new Thread(GraphicsFunc);
            this.renderThread.Name = "RenderThread";

            this.renderContext = new FRenderContext();
            this.graphicsContext = new FD3DGraphicsContext();
            this.renderPipeline = new FUniversalRenderPipeline("UniversalRP");
        }

        public void Start()
        {
            renderThread.Start();
        }

        public void Exit()
        {
            IsLoopExit = true;
            semaphoreG2R.Signal();
            renderThread.Join();
        }

        public void GraphicsFunc()
        {
            bool isInit = true;

            while (!IsLoopExit)
            {
                semaphoreG2R.Wait();
                ProcessGraphicsTasks();
                if (isInit) {
                    isInit = false;
                    renderPipeline.Init(renderContext, graphicsContext); 
                }
                renderPipeline.Render(renderContext, graphicsContext);
                FRHIGraphicsContext.SubmitAndFlushContext(graphicsContext);
                semaphoreR2G.Signal();
            }
        }

        public void ProcessGraphicsTasks()
        {
            if (FGraphics.GraphicsTasks.length == 0) { return; }

            for (int i = 0; i < FGraphics.GraphicsTasks.length; ++i) {
                FGraphics.GraphicsTasks[i](renderContext, graphicsContext);
                FGraphics.GraphicsTasks[i] = null;
            }
            FGraphics.GraphicsTasks.Clear();
        }

        protected override void Release()
        {
            ProcessGraphicsTasks();
            renderPipeline?.Release(renderContext, graphicsContext);

            renderContext?.Dispose();
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
        }
    }
}
