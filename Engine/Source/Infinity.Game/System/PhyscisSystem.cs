using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.System
{
    internal class FPhyscisSystem : FDisposer
    {
        private bool m_LoopExit;
        internal Thread PhyscisThread;

        internal FPhyscisSystem()
        {
            m_LoopExit = false;

            PhyscisThread = new Thread(PhyscisFunc);
            PhyscisThread.Name = "PhyscisThread";
        }

        internal void PhyscisFunc()
        {
            PhyscisLoop();
        }

        internal void Start()
        {
            PhyscisThread.Start();
        }

        internal void Sync()
        {
            PhyscisThread.Join();
        }

        private void PhyscisLoop()
        {
            while (!m_LoopExit)
            {

            }
        }

        internal void Exit()
        {
            m_LoopExit = true;
        }

        protected override void Disposed()
        {

        }
    }
}
