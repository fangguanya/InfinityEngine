using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    [Serializable]
    public class AActor : UObject, IComparable<AActor>, IEquatable<AActor>
    {
        public string Name;
        public AActor Parent;
        internal List<AActor> Childs;
        internal List<UComponent> Components;

        public AActor()
        {
            Name = "";
            Parent = null;
            Childs = new List<AActor>(32);
            Components = new List<UComponent>(8);
        }

        public AActor(string InName)
        {
            Name = InName;
            Parent = null;
            Childs = new List<AActor>(32);
            Components = new List<UComponent>(8);
        }

        public AActor(string InName, AActor InParent)
        {
            Name = InName;
            Parent = InParent;
            Childs = new List<AActor>(32);
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

        public virtual void OnDisable() 
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnDisable();
            }
        }

        public virtual void OnRemove() 
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].OnRemove();
            }
        }

        public bool Equals(AActor other)
        {
            return Name.Equals(other.Name) && Parent.Equals(other.Parent) && Childs.Equals(other.Childs) && Components.Equals(other.Components);
        }

        public int CompareTo(AActor other)
        {
            return 0;
        }

        public void SetParent(AActor InParent)
        {
            Parent = InParent;
        }

        public void AddChildEntity<T>(T InEntity) where T : AActor
        {
            InEntity.Parent = this;
            Childs.Add(InEntity);
        }

        public T FindChildEntity<T>() where T : AActor
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

        public void RemoveChildEntity<T>(T InEntity) where T : AActor
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

        protected override void Disposed()
        {

        }
    }
}
