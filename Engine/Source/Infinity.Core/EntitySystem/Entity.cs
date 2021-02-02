﻿using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    public class AEntity : UObject, IComparable<AEntity>, IEquatable<AEntity>
    {
        public string name;
        internal AEntity ParentEntity;
        internal AEntity[] ChildEntityList;
        internal List<UComponent> ComponentList;

        public AEntity()
        {
            ParentEntity = null;
            ComponentList = new List<UComponent>(8);
        }

        public AEntity(AEntity InParentEntity)
        {
            ParentEntity = InParentEntity;
            ComponentList = new List<UComponent>(8);
        }

        public bool Equals(AEntity other)
        {
            return name.Equals(other.name);
        }

        public int CompareTo(AEntity other)
        {
            return 0;
        }

        public virtual void OnCreate() 
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (!ComponentList[i].bSpawnFlush)
                {
                    ComponentList[i].OnCreate();
                }
            }
        }

        public virtual void OnEnable()
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (!ComponentList[i].bSpawnFlush)
                {
                    ComponentList[i].OnEnable();
                    ComponentList[i].bSpawnFlush = false;
                }
            }
        }

        public virtual void OnTransform() { }

        public virtual void OnUpdate(float frameTime)
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].bSpawnFlush)
                {
                    ComponentList[i].OnCreate();
                    ComponentList[i].OnEnable();
                    ComponentList[i].bSpawnFlush = false;
                }

                ComponentList[i].OnUpdate(frameTime);
            }
        }

        public virtual void OnDisable() { }

        public virtual void OnRemove() { }

        /*public void AddChildEntity<T>(T InEntity) where T : Entity
        {
            InEntity.ParentEntity = this;
            ComponentList.Add(InComponent);
        }

        public void RemoveChildEntity<T>(T InComponent) where T : Entity
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i] == InComponent)
                {
                    ComponentList[i].OnRemove();
                    ComponentList.RemoveAt(i);
                }
            }
        }

        public T GetChildEntity<T>() where T : Entity
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].GetType() == typeof(T))
                {
                    return (T)ComponentList[i];
                }
            }

            return null;
        }*/

        public void SetParent(AEntity InParentEntity)
        {
            ParentEntity = InParentEntity;
        }

        public void AddComponent<T>(T InComponent) where T : UComponent
        {
            InComponent.OwnerEntity = this;
            ComponentList.Add(InComponent);
        }

        public void RemoveComponent<T>(T InComponent) where T : UComponent
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i] == InComponent)
                {
                    ComponentList[i].OnRemove();
                    ComponentList.RemoveAt(i);
                }
            }
        }

        public T GetComponent<T>() where T : UComponent
        {
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].GetType() == typeof(T))
                {
                    return (T)ComponentList[i];
                }
            }

            return null;
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }
}
