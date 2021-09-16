using InfinityEngine.Core.Container;

namespace InfinityEngine.Core.Memory
{
    public delegate void TPooledAction<T0>(T0 arg0);

    public class TObjectPool<T> where T : new()
    {
        readonly Stack<T> m_Stack = new Stack<T>();
        readonly TPooledAction<T> m_ActionOnGet;
        readonly TPooledAction<T> m_ActionOnRelease;
        readonly bool m_CollectionCheck = true;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public TObjectPool(TPooledAction<T> actionOnGet, TPooledAction<T> actionOnRelease, bool collectionCheck = true)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            m_CollectionCheck = collectionCheck;
        }

        public T GetTemporary()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            if (m_ActionOnGet != null)
                m_ActionOnGet(element);
            return element;
        }

        public struct PooledObject : IDisposable
        {
            readonly T m_ToReturn;
            readonly TObjectPool<T> m_Pool;

            internal PooledObject(T value, TObjectPool<T> pool)
            {
                m_ToReturn = value;
                m_Pool = pool;
            }

            void IDisposable.Dispose() => m_Pool.ReleaseTemporary(m_ToReturn);
        }

        public PooledObject GetTemporary(out T v) => new PooledObject(v = GetTemporary(), this);

        public void ReleaseTemporary(T element)
        {
#if WITH_EDITOR // keep heavy checks in editor
            if (m_CollectionCheck && m_Stack.Count > 0)
            {
                if (m_Stack.Contains(element))
                    Console.WriteLine("Internal error. Trying to destroy object that is already released to pool.");
            }
#endif
            if (m_ActionOnRelease != null)
                m_ActionOnRelease(element);
            m_Stack.Push(element);
        }
    }

    public static class TGenericPool<T> where T : new()
    {
        static readonly TObjectPool<T> s_Pool = new TObjectPool<T>(null, null);

        public static T Get() => s_Pool.GetTemporary();

        public static TObjectPool<T>.PooledObject GetTemporary(out T value) => s_Pool.GetTemporary(out value);

        public static void Release(T toRelease) => s_Pool.ReleaseTemporary(toRelease);
    }

    public static class TUnsafeGenericPool<T> where T : new()
    {
        static readonly TObjectPool<T> s_Pool = new TObjectPool<T>(null, null, false);

        public static T Get() => s_Pool.GetTemporary();

        public static TObjectPool<T>.PooledObject GetTemporary(out T value) => s_Pool.GetTemporary(out value);

        public static void Release(T toRelease) => s_Pool.ReleaseTemporary(toRelease);
    }

    public static class TArrayPool<T>
    {
        static readonly TObjectPool<TArray<T>> s_Pool = new TObjectPool<TArray<T>>(null, l => l.Clear());

        public static TArray<T> Get() => s_Pool.GetTemporary();

        public static TObjectPool<TArray<T>>.PooledObject GetTemporary(out TArray<T> value) => s_Pool.GetTemporary(out value);

        public static void Release(TArray<T> toRelease) => s_Pool.ReleaseTemporary(toRelease);
    }

    public static class TSetPool<T>
    {
        static readonly TObjectPool<HashSet<T>> s_Pool = new TObjectPool<HashSet<T>>(null, l => l.Clear());

        public static HashSet<T> Get() => s_Pool.GetTemporary();

        public static TObjectPool<HashSet<T>>.PooledObject GetTemporary(out HashSet<T> value) => s_Pool.GetTemporary(out value);

        public static void Release(HashSet<T> toRelease) => s_Pool.ReleaseTemporary(toRelease);
    }

    public static class TMapPool<TKey, TValue>
    {
        static readonly TObjectPool<Dictionary<TKey, TValue>> s_Pool = new TObjectPool<Dictionary<TKey, TValue>>(null, l => l.Clear());

        public static Dictionary<TKey, TValue> Get() => s_Pool.GetTemporary();

        public static TObjectPool<Dictionary<TKey, TValue>>.PooledObject GetTemporary(out Dictionary<TKey, TValue> value) => s_Pool.GetTemporary(out value);

        public static void Release(Dictionary<TKey, TValue> toRelease) => s_Pool.ReleaseTemporary(toRelease);
    }

}
