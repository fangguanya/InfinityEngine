using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Game.ActorSystem;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Game.Component
{
    public class FPrimitiveProxy : FDisposer
    {
        protected override void Disposed()
        {

        }
    }

    public class UPrimitiveComponent : UComponent
    {
        public UPrimitiveComponent()
        {

        }

        public override void OnEnable()
        {
            OnRegister();
            CreateRender();

            FGraphicsSystem.EnqueueRenderTask(
            (FRHIGraphicsContext graphicsContext, FRenderContext renderContext) =>
            {
                Console.WriteLine("RenderTask");
            });
        }

        public override void OnUpdate()
        {

        }

        public override void OnDisable()
        {
            UnRegister();
            DestroyRender();
        }

        public virtual void OnRegister() { }

        public virtual void UnRegister() { }

        public virtual void CreateRender() { }

        public virtual void UpdateRender() { }

        public virtual void DestroyRender() { }
    }
}
