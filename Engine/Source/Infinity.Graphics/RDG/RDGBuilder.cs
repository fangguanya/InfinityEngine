using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RDG
{
    public class FRDGBuilder : UObject
    {
        public string name;

        public FRDGBuilder(string Name)
        {
            name = Name;
        }

        protected override void Disposed()
        {

        }
    }
}
