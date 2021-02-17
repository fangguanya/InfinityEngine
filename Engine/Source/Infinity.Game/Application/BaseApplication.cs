using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Game.Module;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Game.Application
{
    [Serializable]
    public abstract partial class FBaseApplication : UObject
    {
        internal static readonly string WndClassName = "InfinityApp";

        internal WNDPROC _wndProc;
        internal readonly IntPtr HInstance = Kernel32.GetModuleHandle(null);

        internal FGameModule GameModule;
        internal FRenderModule RenderModule;
        internal FPhyscisModule PhyscisModule;
        internal FWindow MainWindow { get; private set; }


        public FBaseApplication(string Name, int Width, int Height)
        {
            GameModule = new FGameModule(Play, Tick, End);
            RenderModule = new FRenderModule();
            PhyscisModule = new FPhyscisModule();

            CreateWindow(Name, Width, Height);
        }

        public void Run()
        {
            PlatformRun();
            PlatformExit();
        }

        protected virtual void Play()
        {

        }

        protected virtual void Tick()
        {

        }

        protected virtual void End()
        {

        }

        protected override void Disposed()
        {
            GameModule?.Dispose();
            RenderModule?.Dispose();
            PhyscisModule?.Dispose();
        }

        private void CreateWindow(string Name, int Width, int Height)
        {
            _wndProc = ProcessWindowMessage;
            var wndClassEx = new WNDCLASSEX
            {
                Size = Unsafe.SizeOf<WNDCLASSEX>(),
                Styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW | WindowClassStyles.CS_OWNDC,
                WindowProc = _wndProc,
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

            // Create main window.
            MainWindow = new FWindow(Name, Width, Height);
        }

        private void PlatformRun()
        {
            PhyscisModule.Start();
            RenderModule.Start();
            GameModule.Start();
        }

        private void PlatformExit()
        {
            GameModule.Exit();
            RenderModule.Exit();
            RenderModule.Sync();
            PhyscisModule.Exit();
            PhyscisModule.Sync();
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

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return (int)intPtr.ToInt64();
        }
    }
}
