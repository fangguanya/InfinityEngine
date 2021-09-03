using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using System.Collections.Concurrent;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

    public class FGraphicsSystem : FDisposable
    {
        private bool bLoopExit;

        internal Thread renderThread;
        private AutoResetEvent autoEvent;
        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        private static ConcurrentQueue<FGraphicsTask> GraphicsTasks;

        internal FGraphicsSystem(AutoResetEvent autoEvent)
        {
            bLoopExit = false;
            this.autoEvent = autoEvent;
            renderThread = new Thread(GraphicsFunc);
            renderThread.Name = "RenderThread";

            renderContext = new FRenderContext();
            graphicsContext = new FRHIGraphicsContext();
            renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            GraphicsTasks = new ConcurrentQueue<FGraphicsTask>();
        }

        internal void Start()
        {
            renderThread.Start();
        }

        internal void Wiat()
        {
            bLoopExit = true;
            renderThread.Join();
        }

        public static void EnqueueTask(FGraphicsTask graphicsTask)
        {
            GraphicsTasks.Enqueue(graphicsTask);
        }

        private void ProcessGraphicsTasks()
        {
            if(GraphicsTasks.Count == 0) { return; }

            for (int i = 0; i < GraphicsTasks.Count; ++i)
            {
                GraphicsTasks.TryDequeue(out FGraphicsTask graphicsTask);
                graphicsTask(renderContext, graphicsContext);
            }
        }

        private void GraphicsFunc()
        {
            renderPipeline.Init(renderContext, graphicsContext);

            while (!bLoopExit)
            {
                ProcessGraphicsTasks();
                renderPipeline.Render(renderContext, graphicsContext);
                autoEvent.Set();
            }

            ProcessGraphicsTasks();
        }

        internal void Exit()
        {
            renderContext?.Dispose();
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
        }

        protected override void Release()
        {

        }
    }
}
