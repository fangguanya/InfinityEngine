using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Game.System;
using InfinityEngine.Core.Profiler;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Thread.Sync;

namespace InfinityEngine.Game.Application
{
    public abstract partial class FApplication : FDisposable
    {
        public static uint TargetFrameRate = 60;

        internal static readonly string WndClassName = "InfinityApp";

        internal WNDPROC wndProc;
        internal FWindow mainWindow { get; private set; }
        internal readonly IntPtr HInstance = Kernel32.GetModuleHandle(null);

        private FSemaphore m_SemaphoreG2R;
        private FSemaphore m_SemaphoreR2G;

        internal FGameSystem gameSystem;
        internal FPhysicsSystem physicsSystem;
        internal FGraphicsSystem graphicsSystem;

        public FApplication(in int width, in int height, string name = null)
        {
            CreateWindow(width, height, name);
            
            m_SemaphoreR2G = new FSemaphore(true);
            m_SemaphoreG2R = new FSemaphore(false);

            gameSystem = new FGameSystem(End, Play, Tick, m_SemaphoreG2R, m_SemaphoreR2G);
            physicsSystem = new FPhysicsSystem();
            graphicsSystem = new FGraphicsSystem(m_SemaphoreG2R, m_SemaphoreR2G);
        }

        protected abstract void Play();

        protected abstract void Tick();

        protected abstract void End();

        public void Run()
        {
            PlatformRun();
            PlatformExit();
            Release();
        }

        private void PlatformRun()
        {
            gameSystem.Start();
            physicsSystem.Start();
            graphicsSystem.Start();
            gameSystem.GameLoop();
        }

        private void PlatformExit()
        {
            gameSystem.Exit();
            physicsSystem.Exit();
            graphicsSystem.Exit();
        }

        protected override void Release()
        {
            gameSystem.Dispose();
            physicsSystem.Dispose();
            graphicsSystem.Dispose();

            mainWindow.Destroy();
            m_SemaphoreR2G.Dispose();
            m_SemaphoreG2R.Dispose();
        }

        private void CreateWindow(in int width, in int height, string name)
        {
            wndProc = ProcessWindowMessage;
            var wndClassEx = new WNDCLASSEX
            {
                Size = Unsafe.SizeOf<WNDCLASSEX>(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_OWNDC,
                WindowProc = wndProc,
                InstanceHandle = HInstance,
                CursorHandle = User32.LoadCursor(IntPtr.Zero, SystemCursor.IDC_ARROW),
                BackgroundBrushHandle = IntPtr.Zero,
                IconHandle = IntPtr.Zero,
                ClassName = WndClassName,
            };

            if (User32.RegisterClassEx(ref wndClassEx) == 0)
            {
                throw new InvalidOperationException($"Failed to register window class. Error: {Marshal.GetLastWin32Error()}");
            }

            mainWindow = new FWindow(name, width, height);
        }

        private IntPtr ProcessWindowMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == (uint)WindowMessage.ActivateApp)
            {
                return User32.DefWindowProc(hWnd, msg, wParam, lParam);
            }

            switch ((WindowMessage)msg)
            {
                case WindowMessage.Destroy:
                    User32.PostQuitMessage(0);
                    break;
            }

            return User32.DefWindowProc(hWnd, msg, wParam, lParam);
        }
    }
}
