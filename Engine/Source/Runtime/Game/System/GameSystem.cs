using System;
using System.Threading;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Core.Profiler;
using InfinityEngine.Core.Thread.Sync;
using InfinityEngine.Game.Application;

namespace InfinityEngine.Game.System
{
    internal delegate void FGamePlayFunc();
    internal delegate void FGameTickFunc();
    internal delegate void FGameEndFunc();

    internal class FGameSystem : FDisposable
    {
        private bool bLoopExit;
        private float m_DeltaTime;
        private FTimeProfiler m_TimeProfiler;
        private List<float> m_LastDeltaTimes = new List<float>(64);

        private FSemaphore m_SemaphoreG2R;
        private FSemaphore m_SemaphoreR2G;

        private FGameEndFunc m_GameEndFunc;
        private FGamePlayFunc m_GamePlayFunc;
        private FGameTickFunc m_GameTickFunc;

        public FGameSystem(FGameEndFunc gameEndFunc, FGamePlayFunc gamePlayFunc, FGameTickFunc gameTickFunc, FSemaphore semaphoreG2R, FSemaphore semaphoreR2G)
        {
            this.m_GameEndFunc = gameEndFunc;
            this.m_GamePlayFunc = gamePlayFunc;
            this.m_GameTickFunc = gameTickFunc;
            this.m_SemaphoreG2R = semaphoreG2R;
            this.m_SemaphoreR2G = semaphoreR2G;
            this.m_TimeProfiler = new FTimeProfiler();
            Thread.CurrentThread.Name = "GameThread";
        }

        public void Start()
        {
            m_GamePlayFunc();
            m_TimeProfiler.Reset();
            m_TimeProfiler.Start();
        }

        public void Exit()
        {
            m_GameEndFunc();
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

                m_TimeProfiler.Restart();
                m_SemaphoreR2G.Wait();
                FGameTime.Tick(m_DeltaTime);
                m_GameTickFunc();
                m_SemaphoreG2R.Signal();
                ApplyFrameLimitSync();
            }
        }
        
        void ApplyFrameLimitSync()
        {
            long elapsed = 0;

            if (FApplication.TargetFrameRate > 0)
            {
                long targetMax = 1000000L / FApplication.TargetFrameRate;

                for (; ; )
                {
                    elapsed = m_TimeProfiler.microseconds;
                    if (elapsed >= targetMax)
                        break;

                    // Sleep if 1 ms or more off the frame limiting goal
                    if (targetMax - elapsed >= 1000L)
                    {
                        int sleepTime = (int)((targetMax - elapsed) / 1000L);
                        Thread.Sleep(sleepTime);
                    }
                }
            }

            m_TimeProfiler.Restart();
            elapsed = m_TimeProfiler.microseconds;

            // If FPS lower than targetMin, clamp elapsed time
            if (elapsed > 1000000L) { elapsed = 1000000L; }

            // Perform timestep smoothing
            m_DeltaTime = 0.0f;
            int deltaTimeSmoothing = 2;
            m_LastDeltaTimes.Add(elapsed / 1000000.0f);

            if (m_LastDeltaTimes.Count > deltaTimeSmoothing)
            {
                // If the smoothing configuration was changed, ensure correct amount of samples
                m_LastDeltaTimes.RemoveRange(0, m_LastDeltaTimes.Count - deltaTimeSmoothing);
                for (int i = 0; i < m_LastDeltaTimes.Count; ++i)
                {
                    m_DeltaTime += m_LastDeltaTimes[i];
                }
                m_DeltaTime /= m_LastDeltaTimes.Count;
            } else {
                m_DeltaTime = m_LastDeltaTimes[m_LastDeltaTimes.Count - 1];
            }
        }
        
        protected override void Release()
        {
            m_TimeProfiler.Stop();
        }
    }
}
