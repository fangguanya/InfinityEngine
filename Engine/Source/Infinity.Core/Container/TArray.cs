using System;

namespace InfinityEngine.Core.Container
{
    public class TArray<T> /*where T : new()*/
    {
        T[] m_Array = null;

        public int size { get; private set; }

        public TArray()
        {
            m_Array = new T[64];
            size = 64;
        }

        public TArray(int InSize)
        {
            m_Array = new T[InSize];
            size = InSize;
        }

        public void Clear()
        {
            size = 0;
        }

        public int Add(in T value)
        {
            int index = size;

            // Grow array if needed;
            if (index >= m_Array.Length)
            {
                var newArray = new T[m_Array.Length * 2];
                Array.Copy(m_Array, newArray, m_Array.Length);
                m_Array = newArray;
            }

            m_Array[index] = value;
            size++;
            return index;
        }

        public int AddUnique(in T value)
        {
            return 0;
        }

        public void Remove(in T value)
        {

        }

        public void Resize(int newSize, bool keepContent = true)
        {
            if (newSize > m_Array.Length)
            {
                if (keepContent)
                {
                    var newArray = new T[newSize];
                    Array.Copy(m_Array, newArray, m_Array.Length);
                    m_Array = newArray;
                }
                else
                {
                    m_Array = new T[newSize];
                }
            }

            size = newSize;
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
