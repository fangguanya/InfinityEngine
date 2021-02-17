using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Game.Module
{
    internal class FPhyscisModule : UObject
    {
        private bool m_LoopExit;
        internal Thread PhyscisThread;

        internal FPhyscisModule()
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
