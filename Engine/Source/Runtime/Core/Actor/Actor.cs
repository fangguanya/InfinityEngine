using System;
using System.Collections;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.Mathmatics;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Thread.Coroutine;

namespace InfinityEngine.Game.ActorFramework
{
    [Serializable]
    public class AActor : UObject, IComparable<AActor>, IEquatable<AActor>
    {
        public AActor parent;
        public FTransform transform;

        private FTransform m_LastTransform;
        private FCoroutineDispatcher m_CoroutineDispatcher;

        internal TArray<AActor> childs;
        internal TArray<UComponent> components;

        public AActor()
        {
            this.parent = null;
            this.childs = new TArray<AActor>(8);
            this.components = new TArray<UComponent>(8);
            this.m_CoroutineDispatcher = new FCoroutineDispatcher();
        }

        public AActor(string name) : base(name)
        {
            this.parent = null;
            this.childs = new TArray<AActor>(8);
            this.components = new TArray<UComponent>(8);
            this.m_CoroutineDispatcher = new FCoroutineDispatcher();
        }

        public AActor(string name, AActor parent) : base(name)
        {
            this.parent = parent;
            this.childs = new TArray<AActor>(8);
            this.components = new TArray<UComponent>(8);
            this.m_CoroutineDispatcher = new FCoroutineDispatcher();
        }

        public virtual void OnEnable()
        {
            for (int i = 0; i < components.length; ++i)
            {
                components[i].OnEnable();
                components[i].IsConstruct = false;
            }
        }

        public virtual void OnTransform()
        {
            for (int i = 0; i < components.length; ++i)
            {
                components[i].OnTransform();
            }
        }

        public virtual void OnUpdate(in float deltaTime)
        {
            if (!transform.Equals(m_LastTransform)) 
            {
                OnTransform();
                m_LastTransform = transform;
            }

            for (int i = 0; i < components.length; ++i)
            {
                if (components[i].IsConstruct)
                {
                    components[i].OnEnable();
                    components[i].IsConstruct = false;
                }

                components[i].OnUpdate(deltaTime);
            }

            m_CoroutineDispatcher.OnUpdate(deltaTime);
        }

        public virtual void OnDisable() 
        {
            for (int i = 0; i < components.length; ++i)
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
            for (int i = 0; i < components.length; ++i)
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
            for (int i = 0; i < components.length; ++i)
            {
                if (components[i] == component)
                {
                    components.RemoveAtIndex(i);
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
            for (int i = 0; i < childs.length; ++i)
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
            for (int i = 0; i < childs.length; ++i)
            {
                if (childs[i] == child)
                {
                    childs.RemoveAtIndex(i);
                }
            }
        }

        public FCoroutineRef StartCoroutine(in float delay, IEnumerator routine)
        {
            return m_CoroutineDispatcher.Start(delay, routine);
        }

        public FCoroutineRef StartCoroutine(IEnumerator routine)
        {
            return m_CoroutineDispatcher.Start(routine);
        }

        public bool StopCoroutine(IEnumerator routine)
        {
            return m_CoroutineDispatcher.Stop(routine);
        }

        public bool StopCoroutine(in FCoroutineRef routine)
        {
            return m_CoroutineDispatcher.Stop(routine);
        }

        public void StopAllCoroutine()
        {
            m_CoroutineDispatcher.StopAll();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetActorPosition(in float3 position)
        {
            transform.position = position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetActorRotation(in quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}
