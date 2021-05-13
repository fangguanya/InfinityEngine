using System;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.ActorSystem
{
    [Serializable]
    public class UComponent : UObject
    {
        public string Name;
        public AActor Owner;
        internal bool bSpawnFlush;

        public UComponent()
        {
            Name = "";
            bSpawnFlush = true;
        }

        public UComponent(string InName)
        {
            Name = InName;
            Owner = null;
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
