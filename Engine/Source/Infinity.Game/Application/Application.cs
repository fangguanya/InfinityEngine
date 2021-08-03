using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Game.System;
using InfinityEngine.Core.Profiler;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Game.Application
{
    [Serializable]
    public abstract partial class FApplication : FDisposable
    {
        internal static readonly string WndClassName = "InfinityApp";

        internal WNDPROC wndProc;
        internal readonly IntPtr HInstance = Kernel32.GetModuleHandle(null);
        internal FWindow mainWindow { get; private set; }

        internal FTimeProfiler timeProfiler;

        internal FGameSystem gameSystem;
        internal FPhyscisSystem physcisSystem;
        internal FGraphicsSystem graphicsSystem;

        public FApplication(string Name, int Width, int Height)
        {
            timeProfiler = new FTimeProfiler();
            gameSystem = new FGameSystem(Play, Tick, End);
            physcisSystem = new FPhyscisSystem();
            graphicsSystem = new FGraphicsSystem();
            CreateWindow(Name, Width, Height);
        }

        protected abstract void Play();

        protected abstract void Tick();

        protected abstract void End();

        public void Run()
        {
            PlatformRun();
            PlatformExit();
        }

        private void PlatformRun()
        {
            timeProfiler.Reset();
            timeProfiler.Start();

            physcisSystem.Start();
            graphicsSystem.Start();
            gameSystem.Start();
        }

        private void PlatformExit()
        {
            gameSystem.Exit();
            mainWindow.Destroy();

            physcisSystem.Wiat();
            physcisSystem.Exit();
            graphicsSystem.Wiat();
            graphicsSystem.Exit();
        }

        protected override void Disposed()
        {
            gameSystem?.Dispose();
            physcisSystem?.Dispose();
            graphicsSystem?.Dispose();
        }

        private void CreateWindow(string name, int width, int height)
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
