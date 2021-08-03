using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Mathmatics;

namespace InfinityEngine.Game.ActorSystem
{
    [Serializable]
    public class AActor : UObject, IComparable<AActor>, IEquatable<AActor>
    {
        public AActor parent;
        public FTransform transform;

        private FTransform m_LastTransform;

        internal List<AActor> childs;
        internal List<UComponent> components;

        public AActor()
        {
            this.parent = null;
            this.childs = new List<AActor>(8);
            this.components = new List<UComponent>(8);
        }

        public AActor(string name) : base(name)
        {
            this.parent = null;
            this.childs = new List<AActor>(8);
            this.components = new List<UComponent>(8);
        }

        public AActor(string name, AActor parent) : base(name)
        {
            this.parent = parent;
            this.childs = new List<AActor>(8);
            this.components = new List<UComponent>(8);
        }

        public virtual void OnEnable()
        {
            for (int i = 0; i < components.Count; ++i)
            {
                components[i].OnEnable();
                components[i].bConstruct = false;
            }
        }

        public virtual void OnTransform()
        {
            for (int i = 0; i < components.Count; ++i)
            {
                components[i].OnTransform();
            }
        }

        public virtual void OnUpdate()
        {
            if(!transform.Equals(m_LastTransform)) 
            {
                OnTransform();
                m_LastTransform = transform;
            }

            for (int i = 0; i < components.Count; ++i)
            {
                if (components[i].bConstruct)
                {
                    components[i].OnEnable();
                    components[i].bConstruct = false;
                }

                components[i].OnUpdate();
            }
        }

        public virtual void OnDisable() 
        {
            for (int i = 0; i < components.Count; ++i)
            {
                components[i].OnDisable();
            }
        }

        public bool Equals(AActor target)
        {
            return name.Equals(target.name) && parent.Equals(target.parent) && childs.Equals(target.childs) && components.Equals(target.components) && transform.Equals(target.transform);
        }

        public int CompareTo(AActor target)
        {
            return 0;
        }

        public void SetParent(AActor parent)
        {
            this.parent = parent;
        }

        public void AddComponent<T>(T component) where T : UComponent
        {
            component.owner = this;
            components.Add(component);
        }

        public T FindComponent<T>() where T : UComponent
        {
            for (int i = 0; i < components.Count; ++i)
            {
                if (components[i].GetType() == typeof(T))
                {
                    return (T)components[i];
                }
            }

            return null;
        }

        public void RemoveComponent<T>(T component) where T : UComponent
        {
            for (int i = 0; i < components.Count; ++i)
            {
                if (components[i] == component)
                {
                    components.RemoveAt(i);
                }
            }
        }

        public void AddChildActor<T>(T child) where T : AActor
        {
            child.parent = this;
            childs.Add(child);
        }

        public T FindChildActor<T>() where T : AActor
        {
            for (int i = 0; i < childs.Count; ++i)
            {
                if (childs[i].GetType() == typeof(T))
                {
                    return (T)childs[i];
                }
            }

            return null;
        }

        public void RemoveChildActor<T>(T child) where T : AActor
        {
            for (int i = 0; i < childs.Count; ++i)
            {
                if (childs[i] == child)
                {
                    childs.RemoveAt(i);
                }
            }
        }
    
        public void SetActorPosition(in float3 position)
        {
            transform.position = position;
        }
    }
}
