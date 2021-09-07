using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Game.System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Game.Application
{
    public abstract partial class FApplication : FDisposable
    {
        internal static readonly string WndClassName = "InfinityApp";

        internal WNDPROC wndProc;
        internal FWindow mainWindow { get; private set; }
        internal readonly IntPtr HInstance = Kernel32.GetModuleHandle(null);

        private AutoResetEvent renderEvent;

        internal FGameSystem gameSystem;
        internal FPhysicsSystem physicsSystem;
        internal FGraphicsSystem graphicsSystem;

        public FApplication(string Name, int Width, int Height)
        {
            renderEvent = new AutoResetEvent(false);
            gameSystem = new FGameSystem(End, Play, Tick, renderEvent);
            physicsSystem = new FPhysicsSystem();
            graphicsSystem = new FGraphicsSystem(renderEvent);
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
            gameSystem.Start();
            physicsSystem.Start();
            graphicsSystem.Start();
            gameSystem.GameLoop();
        }

        private void PlatformExit()
        {
            gameSystem.Exit();
            physicsSystem.Wiat();
            physicsSystem.Exit();
            graphicsSystem.Wiat();
            graphicsSystem.Exit();
            mainWindow.Destroy();
        }

        protected override void Release()
        {
            gameSystem?.Dispose();
            physicsSystem?.Dispose();
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
