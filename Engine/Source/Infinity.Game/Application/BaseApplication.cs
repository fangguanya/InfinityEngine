﻿using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Game.Window;
using InfinityEngine.Game.System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Game.Application
{
    [Serializable]
    public abstract partial class FBaseApplication : FDisposer
    {
        internal static readonly string WndClassName = "InfinityApp";

        internal WNDPROC wndProc;
        internal readonly IntPtr HInstance = Kernel32.GetModuleHandle(null);
        internal FWindow mainWindow { get; private set; }

        internal FGameSystem gameSystem;
        internal FPhyscisSystem physcisSystem;
        internal FGraphicsSystem graphicsSystem;


        public FBaseApplication(string Name, int Width, int Height)
        {
            gameSystem = new FGameSystem(Play, Tick, End);
            physcisSystem = new FPhyscisSystem();
            graphicsSystem = new FGraphicsSystem();
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

        private void PlatformRun()
        {
            physcisSystem.Start();
            graphicsSystem.Start();
            gameSystem.Start();
        }

        private void PlatformExit()
        {
            gameSystem.Exit();
            physcisSystem.Exit();
            physcisSystem.Sync();
            graphicsSystem.Exit();
            graphicsSystem.Sync();
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
