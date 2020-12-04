using Infinity.Runtime.Graphics.Core;

namespace Infinity.Runtime.Graphics.RDG
{
    public class RenderGraph : TObject
    {
        public string name;

        public RenderGraph(string Name)
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
