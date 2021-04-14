using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderPipeline;

namespace InfinityEngine.Game.Module
{
    public delegate void FGraphicsTask(FRHIGraphicsContext RHIGraphicsContext);

    public class FGraphicsModule : UObject
    {
        private bool b_CopyLoopExit;
        private bool b_RenderLoopExit;
        internal Thread graphicsCopyThread;
        internal Thread graphicsRenderThread;
        internal FRenderPipeline renderPipeline;
        internal FRHIGraphicsContext graphicsContext;

        internal static Queue<FGraphicsTask> GraphicsCopyTasks;
        internal static Queue<FGraphicsTask> GraphicsRenderTasks;


        internal FGraphicsModule()
        {
            b_CopyLoopExit = false;
            b_RenderLoopExit = false;

            graphicsCopyThread = new Thread(GraphicsCopyFunc);
            graphicsCopyThread.Name = "GraphicsCopyThread";
            graphicsRenderThread = new Thread(GraphicsRenderFunc);
            graphicsRenderThread.Name = "GraphicsRenderThread";

            graphicsContext = new FRHIGraphicsContext();
            renderPipeline = new FUniversalRenderPipeline("UniversalRP");

            GraphicsCopyTasks = new Queue<FGraphicsTask>(512);
            GraphicsRenderTasks = new Queue<FGraphicsTask>(512);
        }

        internal void Start()
        {
            graphicsCopyThread.Start();
            graphicsRenderThread.Start();
        }

        internal void Sync()
        {
            graphicsCopyThread.Join();
            graphicsRenderThread.Join();
        }

        #region GraphicsCopy
        public static void EnqueueGraphicsCopyTask(FGraphicsTask graphicsCopyTask)
        {
            GraphicsCopyTasks.Enqueue(graphicsCopyTask);
        }

        private void ProcessGraphicsCopyTask()
        {
            if (GraphicsCopyTasks.Count == 0) { return; }

            for (int i = 0; i < GraphicsCopyTasks.Count; i++)
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

            for(int i = 0; i < GraphicsRenderTasks.Count; i++)
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
