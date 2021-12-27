using System;

namespace InfinityEngine.Core.Object
{
    [Serializable]
    public class UObject : FDisposal
    {
        public string name;

        public UObject()
        {
            name = null;
        }

        public UObject(string name)
        {
            this.name = name;
        }

        protected override void Release() 
        {
            base.Release();
        }
    }
}
