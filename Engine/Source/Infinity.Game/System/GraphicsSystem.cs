using System;
using System.Threading;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

    public class FGraphicsSystem : FDisposer
    {
        private bool bLoopExit;

        internal Thread renderThread;

        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        private static Queue<FGraphicsTask> GraphicsTasks;

        internal FGraphicsSystem()
        {
            bLoopExit = false;

            renderThread = new Thread(GraphicsFunc);
            renderThread.Name = "RenderThread";

            renderContext = new FRenderContext();
            graphicsContext = new FRHIGraphicsContext();
            renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            GraphicsTasks = new Queue<FGraphicsTask>(512);
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

            for(int i = 0; i < GraphicsTasks.Count; ++i)
            {
                FGraphicsTask graphicsTask = GraphicsTasks.Dequeue();
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
            }
        }

        internal void Exit()
        {
            renderContext?.Dispose();
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
            Console.WriteLine("Release Graphics");
        }

        protected override void Disposed()
        {

        }
    }
}
