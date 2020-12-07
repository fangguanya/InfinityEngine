using System;
using System.Drawing;
using System.Windows.Forms;
using InfinityEngine.Core.Object;

namespace ExampleProject
{
    [Serializable]
    class FApplication : UObject
    {
        public bool bRun;
        public Form Window;
        public string Name;

        public FApplication(string InName, int Width, int Height)
        {
            bRun = true;
            Name = InName;

            Window = new Form
            {
                Text = Name,
                Name = Name,
                FormBorderStyle = FormBorderStyle.Sizable,
                ClientSize = new Size(Width, Height),
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new Size(200, 200)
            };
        }

        public void Init()
        {
            Window.Show();
            Window.Update();
        }

        public void Run()
        {
            while (bRun)
            {
                Application.DoEvents();
            }
        }

        public void Release()
        {
            Window.Dispose();
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            Release();
        }
    }
}
