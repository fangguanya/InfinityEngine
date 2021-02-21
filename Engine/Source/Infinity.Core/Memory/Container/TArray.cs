using System;

namespace InfinityEngine.Core.Container
{
    public class TArray<T>
    {
        public int length;
        private T[] m_Array;

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
            // Grow array if needed;
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

        public void RemoveAtIndex(in int index)
        {
            m_Array[index] = m_Array[length - 1];
            m_Array[length - 1] = default(T);
            length--;
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

        public ref T this[int index]
        {
            get
            {
                return ref m_Array[index];
            }
        }
    }
}
