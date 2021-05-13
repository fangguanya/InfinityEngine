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

    internal class FGameSystem : UObject
    {
        private bool m_LoopExit;
        private FGamePlay GamePlay;
        private FGameTick GameTick;
        private FGameEnd GameEnd;

        internal FGameSystem(FGamePlay InGamePlay, FGameTick InGameTick, FGameEnd InGameEnd)
        {
            GamePlay = InGamePlay;
            GameTick = InGameTick;
            GameEnd = InGameEnd;

            Thread.CurrentThread.Name = "GameThread";
        }

        internal void Start()
        {
            GamePlay();
            GameLoop();
        }

        private void GameLoop()
        {
            while (!m_LoopExit)
            {
                if (User32.PeekMessage(out var msg, IntPtr.Zero, 0, 0, 1))
                {
                    User32.TranslateMessage(ref msg);
                    User32.DispatchMessage(ref msg);

                    if (msg.Value == (uint)WindowMessage.Quit)
                    {
                        m_LoopExit = true;
                        break;
                    }
                }

                GameTick();
            }
        }

        internal void Exit()
        {
            GameEnd();
        }

        protected override void Disposed()
        {

        }
    }
}
