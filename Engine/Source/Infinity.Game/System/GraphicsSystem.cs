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
        private bool m_CopyLoopExit;
        private bool m_RenderLoopExit;

        internal Thread copyThread;
        internal Thread renderThread;

        internal FRenderContext renderContext;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        private static Queue<FGraphicsTask> CopyTasks;
        private static Queue<FGraphicsTask> RenderTasks;

        internal FGraphicsSystem()
        {
            m_CopyLoopExit = false;
            m_RenderLoopExit = false;

            copyThread = new Thread(GraphicsCopyFunc);
            copyThread.Name = "CopyThread";
            renderThread = new Thread(GraphicsRenderFunc);
            renderThread.Name = "RenderThread";

            renderContext = new FRenderContext();
            graphicsContext = new FRHIGraphicsContext();
            renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            CopyTasks = new Queue<FGraphicsTask>(512);
            RenderTasks = new Queue<FGraphicsTask>(512);
        }

        internal void Start()
        {
            copyThread.Start();
            renderThread.Start();
        }

        internal void Sync()
        {
            copyThread.Join();
            renderThread.Join();
        }

        #region GraphicsCopy
        public static void EnqueueCopyTask(FGraphicsTask copyTask)
        {
            CopyTasks.Enqueue(copyTask);
        }

        private void ProcessCopyTask()
        {
            if (CopyTasks.Count == 0) { return; }

            for (int i = 0; i < CopyTasks.Count; ++i)
            {
                FGraphicsTask graphicsCopyTask = CopyTasks.Dequeue();
                graphicsCopyTask(graphicsContext, renderContext);
            }
        }

        private void GraphicsCopyFunc()
        {
            while (!m_CopyLoopExit)
            {
                ProcessCopyTask();
            }
        }
        #endregion //GraphicsCopy

        #region GraphicsRender
        public static void EnqueueRenderTask(FGraphicsTask renderTask)
        {
            RenderTasks.Enqueue(renderTask);
        }

        private void ProcessRenderTask()
        {
            if(RenderTasks.Count == 0) { return; }

            for(int i = 0; i < RenderTasks.Count; ++i)
            {
                FGraphicsTask graphicsRenderTask = RenderTasks.Dequeue();
                graphicsRenderTask(graphicsContext, renderContext);
            }
        }

        private void GraphicsRenderFunc()
        {
            renderPipeline.Init(graphicsContext);

            while (!m_RenderLoopExit)
            {
                ProcessRenderTask();
                renderPipeline.Render(graphicsContext);
            }
        }
        #endregion //GraphicsRender

        internal void Exit()
        {
            m_CopyLoopExit = true;
            m_RenderLoopExit = true;
            renderContext?.Dispose();
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
        }

        protected override void Disposed()
        {

        }
    }
}
