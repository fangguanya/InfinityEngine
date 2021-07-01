﻿using System;

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
            m_Array = new TArray<T>();
            m_PoolArray = new TArray<int>();
        }

        public TSparseArray(int capacity)
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
}
