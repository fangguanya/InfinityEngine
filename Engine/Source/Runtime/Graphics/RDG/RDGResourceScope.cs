using System.Collections.Generic;

namespace InfinityEngine.Graphics.RDG
{
    internal class FRDGResourceScope<Type> where Type : struct
    {
        internal Dictionary<int, Type> resourceMap;

        internal FRDGResourceScope()
        {
            resourceMap = new Dictionary<int, Type>(64);
        }

        internal void Set(in int key, in Type value)
        {
            resourceMap.TryAdd(key, value);
        }

        internal Type Get(in int key)
        {
            Type output;
            resourceMap.TryGetValue(key, out output);
            return output;
        }

        internal void Clear()
        {
            resourceMap.Clear();
        }

        internal void Dispose()
        {
            resourceMap = null;
        }
    }
}
