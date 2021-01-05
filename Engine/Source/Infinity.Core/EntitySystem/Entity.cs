using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Core.EntitySystem
{
    internal struct CastHelper<T>
    {
        public T t;
        public System.IntPtr onePointerFurtherThanT;
    }

    public class Entity : UObject, IComparable<Entity>, IEquatable<Entity>
    {
        public string name;
        internal Entity ParentEntity;
        internal Entity[] ChildEntityList;
        internal List<Component> ComponentList;

        public Entity()
        {

        }

        public bool Equals(Entity other)
        {
            return name.Equals(other.name);
        }

        public int CompareTo(Entity other)
        {
            return 0;
        }

        public virtual void OnCreate() { }

        public virtual void OnEnable() { }

        protected virtual void OnTransformChanged() { }

        public virtual void OnGameUpdate(float frameTime) { }

        public virtual void OnEditorUpdate(float frameTime) { }

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

        public void AddComponent<T>(T InComponent) where T : Component
        {
            InComponent.OwnerEntity = this;
            ComponentList.Add(InComponent);
        }

        public void RemoveComponent<T>(T InComponent) where T : Component
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

        public T GetComponent<T>() where T : Component
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
