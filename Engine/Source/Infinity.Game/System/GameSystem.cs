using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;

namespace InfinityEngine.Game.System
{
    internal delegate void FGamePlay();
    internal delegate void FGameTick();
    internal delegate void FGameEnd();

    internal class FGameSystem : FDisposer
    {
        private bool bLoopExit;
        private FGamePlay gamePlayFunc;
        private FGameTick gameTickFunc;
        private FGameEnd gameEndFunc;

        internal FGameSystem(FGamePlay gamePlayFunc, FGameTick gameTickFunc, FGameEnd gameEndFunc)
        {
            this.gamePlayFunc = gamePlayFunc;
            this.gameTickFunc = gameTickFunc;
            this.gameEndFunc = gameEndFunc;
            Thread.CurrentThread.Name = "GameThread";
        }

        internal void Start()
        {
            gamePlayFunc();
            GameLoop();
        }

        private void GameLoop()
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
