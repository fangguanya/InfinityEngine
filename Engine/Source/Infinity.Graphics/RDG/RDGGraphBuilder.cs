using Infinity.Runtime.Graphics.Core;

namespace Infinity.Runtime.Graphics.RDG
{
    public class RDGGraphBuilder : TObject
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
