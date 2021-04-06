using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RDG
{
    public class FRDGGraphBuilder : UObject
    {
        public string name;

        public FRDGGraphBuilder(string Name)
        {
            name = Name;
        }

        protected override void Disposed()
        {

        }
    }
}
