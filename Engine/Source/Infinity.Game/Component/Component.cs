using System;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.ActorSystem
{
    [Serializable]
    public class UComponent : UObject
    {
        public AActor owner;
        internal bool isConstruct;

        public UComponent()
        {
            owner = null;
            isConstruct = true;
        }

        public UComponent(string name) : base(name)
        {
            owner = null;
            isConstruct = true;
        }

        public virtual void OnEnable() { }

        public virtual void OnTransform() { }

        public virtual void OnUpdate() { }

        public virtual void OnDisable() { }

        protected override void Disposed()
        {

        }
    }
}
