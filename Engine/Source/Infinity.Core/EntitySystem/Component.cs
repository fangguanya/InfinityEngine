
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    public class Component : UObject
    {
        internal Entity OwnerEntity;

        public Component()
        {

        }

        public virtual void OnCreate() { }

        public virtual void OnEnable() { }

        protected virtual void OnTransformChanged() { }

        public virtual void OnGameUpdate(float frameTime) { }

        public virtual void OnEditorUpdate(float frameTime) { }

        public virtual void OnDisable() { }

        public virtual void OnRemove() { }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }
}
