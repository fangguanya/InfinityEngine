using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Game.Module
{
    public delegate void FRenderTask(FRHIRenderContext RHIRenderContext);

    public class FRenderModule : UObject
    {
        private bool m_LoopExit;
        internal Thread RenderThread;
        internal FRHIRenderContext RenderContext;

        internal static Queue<FRenderTask> RenderTasks;

        internal FRenderModule()
        {
            m_LoopExit = false;

            RenderThread = new Thread(RenderFunc);
            RenderThread.Name = "RenderThread";

            RenderTasks = new Queue<FRenderTask>(512);

            RenderContext = new FRHIRenderContext();
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

        private void ProcessRenderTask()
        {
            for(int i = 0; i < RenderTasks.Count; i++)
            {
                FRenderTask RenderTask = RenderTasks.Dequeue();
                RenderTask(RenderContext);
            }
        }

        private void RenderLoop()
        {
            while (!m_LoopExit)
            {
                ProcessRenderTask();
            }
        }

        internal void Exit()
        {
            m_LoopExit = true;
            RenderContext?.Dispose();
        }

        protected override void Disposed()
        {

        }
    }
}
