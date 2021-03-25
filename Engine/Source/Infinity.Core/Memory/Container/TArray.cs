using System;

namespace InfinityEngine.Core.Container
{
    public class TArray<T>
    {
        public int length;
        private T[] m_Array;
        

        public ref T this[int index]
        {
            get
            {
                return ref m_Array[index];
            }
        }
        
        public TArray()
        {
            length = 0;
            m_Array = new T[64];
        }

        public TArray(int capacity)
        {
            length = 0;
            m_Array = new T[capacity];
        }

        public void Clear()
        {
            length = 0;
        }

        public int Add(in T value)
        {
            if (length >= m_Array.Length)
            {
                var newArray = new T[m_Array.Length * 2];
                Array.Copy(m_Array, newArray, m_Array.Length);
                m_Array = newArray;
            }

            m_Array[length] = value;
            length++;

            return length;
        }

        public int AddUnique(in T value)
        {
            for(int i = 0; i < length; ++i)
            {
                if (value.Equals(m_Array[i]))
                {
                    return -1;
                }
            }
            Add(value);

            return 0;
        }

        public void Remove(in T value)
        {
            for (int i = 0; i < length; ++i)
            {
                if (value.Equals(m_Array[i]))
                {
                    RemoveAtIndex(i);
                    break;
                }
            }
        }
        
        public void RemoveAtIndex(in int index)
        {
            int lastIndex = length - 1;
            if (index > lastIndex) { return; }

            Array.Copy(m_Array, index + 1, m_Array, index, lastIndex - index);

            m_Array[lastIndex] = default(T);
            length--;
        }

        public void RemoveSwap(in T value)
        {
            for (int i = 0; i < length; ++i)
            {
                if (value.Equals(m_Array[i]))
                {
                    RemoveSwapAtIndex(i);
                    break;
                }
            }
        }
        
        public void RemoveSwapAtIndex(in int index)
        {
            m_Array[index] = m_Array[length - 1];
            m_Array[length - 1] = default(T);
            length--;
        }

        public void Resize(int newLength, bool keepContent = true)
        {
            if (newLength > m_Array.Length)
            {
                if (keepContent)
                {
                    var newArray = new T[newLength];
                    Array.Copy(m_Array, newArray, m_Array.Length);
                    m_Array = newArray;
                } else {
                    m_Array = new T[newLength];
                }
            }

            length = newLength;
        }
    }
}
