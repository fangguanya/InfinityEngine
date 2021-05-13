using System.Threading;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.System
{
    public delegate void FGraphicsTask(FRHIGraphicsContext RHIGraphicsContext);

    public class FGraphicsSystem : FDisposer
    {
        private bool b_CopyLoopExit;
        private bool b_RenderLoopExit;
        internal Thread copyThread;
        internal Thread renderThread;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        internal static Queue<FGraphicsTask> GraphicsCopyTasks;
        internal static Queue<FGraphicsTask> GraphicsRenderTasks;

        internal FGraphicsSystem()
        {
            b_CopyLoopExit = false;
            b_RenderLoopExit = false;

            copyThread = new Thread(GraphicsCopyFunc);
            copyThread.Name = "CopyThread";
            renderThread = new Thread(GraphicsRenderFunc);
            renderThread.Name = "RenderThread";

            graphicsContext = new FRHIGraphicsContext();
            renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            GraphicsCopyTasks = new Queue<FGraphicsTask>(512);
            GraphicsRenderTasks = new Queue<FGraphicsTask>(512);
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
        public static void EnqueueGraphicsCopyTask(FGraphicsTask graphicsCopyTask)
        {
            GraphicsCopyTasks.Enqueue(graphicsCopyTask);
        }

        private void ProcessGraphicsCopyTask()
        {
            if (GraphicsCopyTasks.Count == 0) { return; }

            for (int i = 0; i < GraphicsCopyTasks.Count; ++i)
            {
                FGraphicsTask graphicsCopyTask = GraphicsCopyTasks.Dequeue();
                graphicsCopyTask(graphicsContext);
            }
        }

        private void GraphicsCopyFunc()
        {
            while (!b_CopyLoopExit)
            {
                ProcessGraphicsCopyTask();
            }
        }
        #endregion //GraphicsCopy

        #region GraphicsRender
        public static void EnqueueGraphicsRenderTask(FGraphicsTask graphicsRenderTask)
        {
            GraphicsRenderTasks.Enqueue(graphicsRenderTask);
        }

        private void ProcessGraphicsRenderTask()
        {
            if(GraphicsRenderTasks.Count == 0) { return; }

            for(int i = 0; i < GraphicsRenderTasks.Count; ++i)
            {
                FGraphicsTask graphicsRenderTask = GraphicsRenderTasks.Dequeue();
                graphicsRenderTask(graphicsContext);
            }
        }

        private void GraphicsRenderFunc()
        {
            renderPipeline.Init(graphicsContext);

            while (!b_RenderLoopExit)
            {
                ProcessGraphicsRenderTask();
                renderPipeline.Render(graphicsContext);
            }
        }
        #endregion //GraphicsRender

        internal void Exit()
        {
            b_CopyLoopExit = true;
            b_RenderLoopExit = true;
            renderPipeline?.Dispose();
            graphicsContext?.Dispose();
        }

        protected override void Disposed()
        {

        }
    }
}
