using System;
using System.Threading;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;

namespace InfinityEngine.Game.System
{
    internal delegate void FGamePlayFunc();
    internal delegate void FGameTickFunc();
    internal delegate void FGameEndFunc();

    internal class FGameSystem : FDisposable
    {
        private bool bLoopExit;
        private FGameEndFunc gameEndFunc;
        private FGamePlayFunc gamePlayFunc;
        private FGameTickFunc gameTickFunc;
        private AutoResetEvent autoEvent;

        internal FGameSystem(FGameEndFunc gameEndFunc, FGamePlayFunc gamePlayFunc, FGameTickFunc gameTickFunc, AutoResetEvent autoEvent)
        {
            this.autoEvent = autoEvent;
            this.gameEndFunc = gameEndFunc;
            this.gamePlayFunc = gamePlayFunc;
            this.gameTickFunc = gameTickFunc;
            Thread.CurrentThread.Name = "GameThread";
        }

        internal void Start()
        {
            gamePlayFunc();
        }

        internal void GameLoop()
        {
            while (!bLoopExit)
            {
                if (User32.PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
                {
                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);

                    if (msg.Value == (uint)WindowMessage.Quit)
                    {
                        bLoopExit = true;
                        break;
                    }
                }

                gameTickFunc();
                autoEvent.WaitOne();
            }
        }

        internal void Exit()
        {
            gameEndFunc();
        }

        protected override void Disposed()
        {

        }
    }
}
