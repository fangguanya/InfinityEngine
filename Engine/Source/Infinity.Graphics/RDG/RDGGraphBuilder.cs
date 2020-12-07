using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RDG
{
    public class RDGGraphBuilder : UObject
    {
        public string name;

        public RDGGraphBuilder(string Name)
        {
            name = Name;
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }

    }
}
