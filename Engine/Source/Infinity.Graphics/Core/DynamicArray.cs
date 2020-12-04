using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinity.Runtime.Graphics.Core
{
    public class DynamicArray<T> /*where T : new()*/
    {
        T[] m_Array = null;

        public int size { get; private set; }

        public DynamicArray()
        {
            m_Array = new T[64];
            size = 64;
        }

        public DynamicArray(int InSize)
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

        public void Resize(int newSize, bool keepContent = false)
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
