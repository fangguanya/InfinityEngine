using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using System.Collections.Concurrent;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

    public static class FGraphics
    {
        public static void EnqueueTask(FGraphicsTask graphicsTask)
        {
            FGraphicsSystem.GraphicsTasks.Enqueue(graphicsTask);
        }
    }

    public class FGraphicsSystem : FDisposable
    {
        private bool bLoopExit;

        internal Thread renderThread;
        internal AutoResetEvent autoEvent;
        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        internal static ConcurrentQueue<FGraphicsTask> GraphicsTasks;

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

        internal void GraphicsFunc()
        {
            renderPipeline.Init(renderContext, graphicsContext);
            //graphicsContext.WaitGPU();

            while (!bLoopExit)
            {
                ProcessGraphicsTasks();

                renderPipeline.Render(renderContext, graphicsContext);
                graphicsContext.WaitGPU();

                autoEvent.Set();
            }

            ProcessGraphicsTasks();
        }

        internal void ProcessGraphicsTasks()
        {
            if (GraphicsTasks.Count == 0) { return; }

            for (int i = 0; i < GraphicsTasks.Count; ++i)
            {
                GraphicsTasks.TryDequeue(out FGraphicsTask graphicsTask);
                #pragma warning disable CS8602
                graphicsTask(renderContext, graphicsContext); // 解引用可能出现空引用。
                #pragma warning restore CS8602
            }
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
