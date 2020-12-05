using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RDG
{
    public class RDGGraphBuilder : FObject
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
