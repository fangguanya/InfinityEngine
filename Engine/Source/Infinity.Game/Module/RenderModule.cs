using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.Module
{
    public delegate void FRenderTask(FRHIGraphicsContext RHIGraphicsContext);

    public class FRenderModule : UObject
    {
        private bool m_LoopExit;
        internal Thread RenderThread;
        internal FRenderPipeline RenderPipeline;
        internal FRHIGraphicsContext GraphicsContext;

        internal static Queue<FRenderTask> RenderTasks;

        internal FRenderModule()
        {
            m_LoopExit = false;

            RenderThread = new Thread(RenderFunc);
            RenderThread.Name = "RenderThread";

            RenderTasks = new Queue<FRenderTask>(512);

            GraphicsContext = new FRHIGraphicsContext();
            RenderPipeline = new FUniversalRenderPipeline("UniversalRP");
        }

        internal void RenderFunc()
        {
            RenderLoop();
        }

        public static void EnqueueRenderTask(FRenderTask RenderTask)
        {
            RenderTasks.Enqueue(RenderTask);
        }

        internal void Start()
        {
            RenderThread.Start();
        }

        internal void Sync()
        {
            RenderThread.Join();
        }

        private void ProcessRenderTask()
        {
            for(int i = 0; i < RenderTasks.Count; i++)
            {
                FRenderTask RenderTask = RenderTasks.Dequeue();
                RenderTask(GraphicsContext);
            }
        }

        private void RenderLoop()
        {
            RenderPipeline.Init(GraphicsContext);

            while (!m_LoopExit)
            {
                ProcessRenderTask();

                RenderPipeline.Render(GraphicsContext);
            }
        }

        internal void Exit()
        {
            m_LoopExit = true;
            GraphicsContext?.Dispose();
            RenderPipeline?.Dispose();
        }

        protected override void Disposed()
        {

        }
    }
}
