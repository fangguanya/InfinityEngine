using System;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.ActorFramework
{
    [Serializable]
    public class UComponent : UObject
    {
        public AActor owner;
        internal bool IsConstruct;

        public UComponent()
        {
            owner = null;
            IsConstruct = true;
        }

        public UComponent(string name) : base(name)
        {
            owner = null;
            IsConstruct = true;
        }

        public virtual void OnEnable() { }

        public virtual void OnTransform() { }

        public virtual void OnUpdate(in float deltaTime) { }

        public virtual void OnDisable() { }

        protected override void Release()
        {

        }
    }
}
