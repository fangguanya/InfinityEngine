using System;
using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Core.Thread.Sync;

namespace InfinityEngine.Game.System
{
    internal delegate void FGamePlayFunc();
    internal delegate void FGameTickFunc();
    internal delegate void FGameEndFunc();

    internal class FGameSystem : FDisposable
    {
        private bool bLoopExit;
        private FSemaphore semaphoreG2R;
        internal FSemaphore semaphoreR2G;
        private FGameEndFunc gameEndFunc;
        private FGamePlayFunc gamePlayFunc;
        private FGameTickFunc gameTickFunc;

        public FGameSystem(FGameEndFunc gameEndFunc, FGamePlayFunc gamePlayFunc, FGameTickFunc gameTickFunc, FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            this.semaphoreG2R = semaphoreG2R;
            this.semaphoreR2G = semaphoreR2G;
            this.gameEndFunc = gameEndFunc;
            this.gamePlayFunc = gamePlayFunc;
            this.gameTickFunc = gameTickFunc;
            Thread.CurrentThread.Name = "GameThread";
        }

        public void Start()
        {
            gamePlayFunc();
        }

        public void Exit()
        {
            gameEndFunc();
        }

        public void GameLoop()
        {
            while (!bLoopExit)
            {
                if (User32.PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
                {
                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);
                    if (msg.Value == (uint)WindowMessage.Quit) { bLoopExit = true; break; }
                }

                semaphoreR2G.Wait();
                gameTickFunc();
                semaphoreG2R.Signal();
            }
        }

        protected override void Release()
        {

        }
    }
}
