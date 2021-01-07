using RemoteControl.Display;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteControl.Hooks
{
    public class Win32
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Vector2 lpPoint);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Vector2 point);

        [DllImport("User32.Dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static int mouseXPos = 0;
        public static int mouseYPos = 0;

        public static void MouseClick(Vector2 pos)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (int)pos.X, (int)pos.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (int)pos.X, (int)pos.Y, 0, 0);
        }

        public static void MouseClick()
        {
            Vector2 lpPos;
            GetCursorPos(out lpPos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, (int)lpPos.X, (int)lpPos.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (int)lpPos.X, (int)lpPos.Y, 0, 0);
        }
    }
}
