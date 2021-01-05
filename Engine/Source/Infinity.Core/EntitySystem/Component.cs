
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    public class UComponent : UObject
    {
        internal AEntity OwnerEntity;

        public UComponent()
        {

        }

        public virtual void OnCreate() { }

        public virtual void OnEnable() { }

        protected virtual void OnTransformChanged() { }

        public virtual void OnUpdate(float frameTime) { }

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
