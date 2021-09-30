using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using System.Collections.Concurrent;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Thread.Sync;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

    public static class FGraphics
    {
        public static void EnqueueTask(FGraphicsTask graphicsTask)
        {
            FGraphicsSystem.GraphicsTasks.Add(graphicsTask);
        }
    }

    internal class FGraphicsSystem : FDisposable
    {
        private bool bLoopExit;

        internal Thread renderThread;
        internal FSemaphore semaphoreG2R;
        internal FSemaphore semaphoreR2G;
        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        internal static TArray<FGraphicsTask> GraphicsTasks;

        public FGraphicsSystem(FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            this.bLoopExit = false;
            this.semaphoreG2R = semaphoreG2R;
            this.semaphoreR2G = semaphoreR2G;
            this.renderThread = new Thread(GraphicsFunc);
            this.renderThread.Name = "RenderThread";

            this.renderContext = new FRenderContext();
            this.graphicsContext = new FRHIGraphicsContext();
            this.renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            FGraphicsSystem.GraphicsTasks = new TArray<FGraphicsTask>(64);
        }

        public void Start()
        {
            renderThread.Start();
        }

        public void Exit()
        {
            bLoopExit = true;
            semaphoreG2R.Signal();
            renderThread.Join();
        }

        public void GraphicsFunc()
        {
            renderPipeline.Init();

            while (!bLoopExit)
            {
                semaphoreG2R.Wait();
                ProcessGraphicsTasks();
                renderPipeline.Render(renderContext, graphicsContext);
                graphicsContext.Flush();
                semaphoreR2G.Signal();
            }
        }

        public void ProcessGraphicsTasks()
        {
            if (GraphicsTasks.length == 0) { return; }

            for (int i = 0; i < GraphicsTasks.length; ++i)
            {
                GraphicsTasks[i](renderContext, graphicsContext);
                GraphicsTasks[i] = null;
            }
            GraphicsTasks.Clear();
        }

        protected override void Release()
        {
            ProcessGraphicsTasks();
            renderContext?.Dispose();
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
        }
    }
}
