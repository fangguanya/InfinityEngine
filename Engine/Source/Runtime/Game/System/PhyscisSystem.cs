using System;
using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.System
{
    internal class FPhysicsSystem : FDisposable
    {
        private bool bLoopExit;
        internal Thread physicsThread;

        public FPhysicsSystem()
        {
            this.bLoopExit = false;
            this.physicsThread = new Thread(PhysicsFunc);
            this.physicsThread.Name = "PhyscisThread";
        }

        public void Start()
        {
            physicsThread.Start();
        }

        public void Exit()
        {
            bLoopExit = true;
            physicsThread.Join();
        }

        public void PhysicsFunc()
        {
            while (!bLoopExit)
            {
                Console.WriteLine("Physics");
            }
        }

        protected override void Release()
        {

        }
    }
}
