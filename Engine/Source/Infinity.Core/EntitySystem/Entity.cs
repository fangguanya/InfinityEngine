using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    [Serializable]
    public class AEntity : UObject, IComparable<AEntity>, IEquatable<AEntity>
    {
        public string Name;
        public AEntity Parent;
        internal List<AEntity> Childs;
        internal List<UComponent> Components;

        public AEntity()
        {
            Name = "";
            Parent = null;
            Childs = new List<AEntity>(32);
            Components = new List<UComponent>(8);
        }

        public AEntity(string InName)
        {
            Name = InName;
            Parent = null;
            Childs = new List<AEntity>(32);
            Components = new List<UComponent>(8);
        }

        public AEntity(string InName, AEntity InParent)
        {
            Name = InName;
            Parent = InParent;
            Childs = new List<AEntity>(32);
            Components = new List<UComponent>(8);
        }

        public virtual void OnCreate() 
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (!Components[i].bSpawnFlush)
                {
                    Components[i].OnCreate();
                }
            }
        }

        public virtual void OnEnable()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (!Components[i].bSpawnFlush)
                {
                    Components[i].OnEnable();
                    Components[i].bSpawnFlush = false;
                }
            }
        }

        public virtual void OnTransform() { }

        public virtual void OnUpdate()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].bSpawnFlush)
                {
                    Components[i].OnCreate();
                    Components[i].OnEnable();
                    Components[i].bSpawnFlush = false;
                }

                Components[i].OnUpdate();
            }
        }

        public virtual void OnDisable() { }

        public virtual void OnRemove() { }

        public bool Equals(AEntity other)
        {
            return Name.Equals(other.Name) && Parent.Equals(other.Parent) && Childs.Equals(other.Childs) && Components.Equals(other.Components);
        }

        public int CompareTo(AEntity other)
        {
            return 0;
        }

        public void SetParent(AEntity InParent)
        {
            Parent = InParent;
        }

        public void AddChildEntity<T>(T InEntity) where T : AEntity
        {
            InEntity.Parent = this;
            Childs.Add(InEntity);
        }

        public T FindChildEntity<T>() where T : AEntity
        {
            for (int i = 0; i < Childs.Count; i++)
            {
                if (Childs[i].GetType() == typeof(T))
                {
                    return (T)Childs[i];
                }
            }

            return null;
        }

        public void RemoveChildEntity<T>(T InEntity) where T : AEntity
        {
            for (int i = 0; i < Childs.Count; i++)
            {
                if (Childs[i] == InEntity)
                {
                    Childs[i].OnRemove();
                    Childs.RemoveAt(i);
                }
            }
        }

        public void AddComponent<T>(T InComponent) where T : UComponent
        {
            InComponent.Owner = this;
            Components.Add(InComponent);
        }

        public T FindComponent<T>() where T : UComponent
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].GetType() == typeof(T))
                {
                    return (T)Components[i];
                }
            }

            return null;
        }

        public void RemoveComponent<T>(T InComponent) where T : UComponent
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] == InComponent)
                {
                    Components[i].OnRemove();
                    Components.RemoveAt(i);
                }
            }
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }
}
