using RemoteClient.Hooks;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static RemoteClient.Hooks.Win32;

namespace RemoteClient.Controller
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
            POINT mousePos;
            Win32.GetCursorPos(out mousePos);
            Vector2 d = new Vector2();
            d.X = mousePos.X;
            d.Y = mousePos.Y;
            return d;
        }
    }
}
