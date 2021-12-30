using System;
using System.Drawing;

namespace InfinityEngine.Game.Window
{
    internal class FWindow
    {
        private const int CW_USEDEFAULT = unchecked((int)0x80000000);

        public string title { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public IntPtr handle { get; private set; }

        public FWindow(string title, int width, int height)
        {
            this.title = title;
            this.width = width;
            this.height = height;
            CreateWindowInternal();
        }

        private void CreateWindowInternal()
        {
            var x = 0;
            var y = 0;
            WindowStyles style = 0;
            WindowExStyles styleEx = 0;
            bool resizable = true;
            {
                if (width > 0 && height > 0)
                {
                    var screenWidth = User32.GetSystemMetrics(SystemMetrics.SM_CXSCREEN);
                    var screenHeight = User32.GetSystemMetrics(SystemMetrics.SM_CYSCREEN);

                    // Place the window in the middle of the screen.WS_EX_APPWINDOW
                    x = (screenWidth - width) / 2;
                    y = (screenHeight - height) / 2;
                }

                if (resizable) {
                    style = WindowStyles.WS_OVERLAPPEDWINDOW;
                } else {
                    style = WindowStyles.WS_POPUP | WindowStyles.WS_BORDER | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;
                }

                styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            }
            style |= WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;

            int windowWidth;
            int windowHeight;

            if (width > 0 && height > 0) {
                var rect = new Rectangle(0, 0, width, height);
                windowWidth = rect.Right - rect.Left;
                windowHeight = rect.Bottom - rect.Top;
                User32.AdjustWindowRectEx(ref rect, style, false, styleEx);
            } else {
                x = y = windowWidth = windowHeight = CW_USEDEFAULT;
            }

            IntPtr hwnd = User32.CreateWindowEx((int)styleEx, Application.FApplication.WndClassName, title, (int)style, x, y, windowWidth, windowHeight, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            if (hwnd == IntPtr.Zero) {
                return;
            }

            User32.ShowWindow(hwnd, ShowWindowCommand.Normal);
            handle = hwnd;
            width = windowWidth;
            height = windowHeight;
        }

        public void Destroy()
        {
            if (handle != IntPtr.Zero)
            {
                var destroyHandle = handle;
                handle = IntPtr.Zero;
                User32.DestroyWindow(destroyHandle);
            }
        }
    }
}
