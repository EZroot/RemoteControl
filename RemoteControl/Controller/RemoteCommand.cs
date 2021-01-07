using RemoteControl.Server;
using RemoteControl.Hooks;
using RemoteControl.Display;

using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace RemoteControl.Controller
{
    public class RemoteCommand
    {
        public static void MoveMouse(int x, int y)
        {
            Win32.SetCursorPos(x, y);
        }

        public static void MouseClick()
        {
            Win32.MouseClick();
        }

        public static Vector2 GetMousePosition()
        {
            Vector2 mousePos;
            Win32.GetCursorPos(out mousePos);
            return mousePos;
        }
    }
}
