using InfinityEngine.Core.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InfinityEngine.Core.Container
{
    public class TSparseArray<T>
    {
        public int length
        {
            get
            {
                return m_Array.length;
            }
        }
        public ref T this[int index]
        {
            get
            {
                return ref m_Array[index];
            }
        }

        private TArray<T> m_Array;
        private TArray<int> m_PoolArray;

        public TSparseArray()
        {
            m_Array = new TArray<T>(64);
            m_PoolArray = new TArray<int>(32);
        }

        public TSparseArray(in int capacity = 64)
        {
            m_Array = new TArray<T>(capacity);
            m_PoolArray = new TArray<int>(capacity / 2);
        }

        public int Add(in T value)
        {
            if(m_PoolArray.length != 0)
            {
                int poolIndex = m_PoolArray[m_PoolArray.length - 1];
                m_PoolArray.RemoveSwapAtIndex(m_PoolArray.length - 1);

                m_Array[poolIndex] = value;
                return poolIndex;
            }
            return m_Array.Add(value);
        }

        public void Remove(in int index)
        {
            m_Array[index] = default(T);
            m_PoolArray.Add(index);
        }
    }

    internal unsafe class TValueSparseArrayDebugger<T> where T : unmanaged
    {
        TValueSparseArray<T> m_Target;

        public TValueSparseArrayDebugger(TValueSparseArray<T> target)
        {
            m_Target = target;
        }

        public int Length
        {
            get
            {
                return m_Target.length;
            }
        }

        public List<T> Array
        {
            get
            {
                var result = new List<T>();
                for (int i = 0; i < m_Target.length; ++i)
                {
                    result.Add(m_Target[i]);
                }
                return result;
            }
        }

        public List<int> IndexArray
        {
            get
            {
                var result = new List<int>();
                TValueArray<int> poolArray = *m_Target.m_PoolArray;
                for (int i = 0; i < poolArray.length; ++i)
                {
                    result.Add(poolArray[i]);
                }
                return result;
            }
        }
    }

    [DebuggerTypeProxy(typeof(TValueSparseArrayDebugger<>))]
    public unsafe struct TValueSparseArray<T> : IDisposable where T : unmanaged
    {
        public int length
        {
            get
            {
                return m_Array->length;
            }
        }
        public ref T this[int index]
        {
            get
            {
                TValueArray<T> array = *m_Array;
                return ref array[index];
            }
        }

        internal TValueArray<T>* m_Array;
        internal TValueArray<int>* m_PoolArray;

        public TValueSparseArray(in int capacity = 64)
        {
            m_Array = (TValueArray<T>*)FMemoryUtil.Malloc(sizeof(TValueArray<T>), 1);
            m_PoolArray = (TValueArray<int>*)FMemoryUtil.Malloc(sizeof(TValueArray<int>), 1);
            m_Array->Init(capacity);
            m_PoolArray->Init(capacity / 2);
        }

        public int Add(in T value)
        {
            TValueArray<T> array = *m_Array;
            TValueArray<int> poolArray = *m_PoolArray;

            if (m_PoolArray->length != 0)
            {
                int poolIndex = poolArray[m_PoolArray->length - 1];
                m_PoolArray->RemoveSwapAtIndex(m_PoolArray->length - 1);

                array[poolIndex] = value;
                return poolIndex;
            }
            return m_Array->Add(value);
        }

        public void Remove(in int index)
        {
            TValueArray<T> array = *m_Array;
            array[index] = default(T);
            m_PoolArray->Add(index);
        }

        public void Dispose()
        {
            m_Array->Dispose();
            m_PoolArray->Dispose();

            FMemoryUtil.Free(m_Array);
            FMemoryUtil.Free(m_PoolArray);

            m_Array->length = 0;
            m_PoolArray->length = 0;

            m_Array = null;
            m_PoolArray = null;
        }
    }
}
