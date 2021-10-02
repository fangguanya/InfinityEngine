using System;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.ActorFramework
{
    [Serializable]
    public class UComponent : UObject
    {
        public AActor owner;
        internal bool bConstruct;

        public UComponent()
        {
            owner = null;
            bConstruct = true;
        }

        public UComponent(string name) : base(name)
        {
            owner = null;
            bConstruct = true;
        }

        public virtual void OnEnable() { }

        public virtual void OnTransform() { }

        public virtual void OnUpdate() { }

        public virtual void OnDisable() { }

        protected override void Release()
        {

        }
    }
}
