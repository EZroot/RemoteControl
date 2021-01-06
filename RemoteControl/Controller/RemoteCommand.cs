using RemoteControl.Server;
using RemoteControl.Hooks;
using RemoteControl.Display;

using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
