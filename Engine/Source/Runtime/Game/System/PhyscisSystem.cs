using System;
using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.System
{
    internal class FPhysicsSystem : FDisposable
    {
        private bool bLoopExit;
        internal Thread PhysicsThread;

        internal FPhysicsSystem()
        {
            bLoopExit = false;

            PhysicsThread = new Thread(PhysicsFunc);
            PhysicsThread.Name = "PhyscisThread";
        }

        internal void Start()
        {
            PhysicsThread.Start();
        }

        internal void Wiat()
        {
            bLoopExit = true;
            PhysicsThread.Join();
        }

        private void PhysicsFunc()
        {
            while (!bLoopExit)
            {

            }
        }

        internal void Exit()
        {
            bLoopExit = true;
        }

        protected override void Release()
        {

        }
    }
}
