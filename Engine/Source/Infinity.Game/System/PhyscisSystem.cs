using System;
using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.System
{
    internal class FPhyscisSystem : FDisposer
    {
        private bool bLoopExit;
        internal Thread PhyscisThread;

        internal FPhyscisSystem()
        {
            bLoopExit = false;

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

        internal void Wiat()
        {
            bLoopExit = true;
            PhyscisThread.Join();
        }

        private void PhyscisLoop()
        {
            while (!bLoopExit)
            {

            }
        }

        internal void Exit()
        {
            bLoopExit = true;
        }

        protected override void Disposed()
        {

        }
    }
}
