using System;
using System.Threading;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderLoop;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRHIGraphicsContext graphicsContext, FRenderContext renderContext);

    public class FGraphicsSystem : FDisposer
    {
        private bool m_LoopExit;

        internal Thread renderThread;

        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        private static Queue<FGraphicsTask> GraphicsTasks;

        internal FGraphicsSystem()
        {
            m_LoopExit = false;

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
            m_LoopExit = true;
            renderThread.Join();
        }

        public static void EnqueueTask(FGraphicsTask graphicsTask)
        {
            GraphicsTasks.Enqueue(graphicsTask);
        }

        private void ProcessTasks()
        {
            if(GraphicsTasks.Count == 0) { return; }

            for(int i = 0; i < GraphicsTasks.Count; ++i)
            {
                FGraphicsTask graphicsTask = GraphicsTasks.Dequeue();
                graphicsTask(graphicsContext, renderContext);
            }
        }

        private void GraphicsFunc()
        {
            renderPipeline.Init(graphicsContext);

            while (!m_LoopExit)
            {
                ProcessTasks();
                renderPipeline.Render(graphicsContext);
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
