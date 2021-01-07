using RemoteControl.Server;
using RemoteControl.Hooks;
using RemoteControl.Display;

using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics;

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

        //This returns in chunks for some reason?
        //guess its how process list works? idk
        //need to find workaround
        public static string ListProcesses()
        {
            Process[] processlist = Process.GetProcesses();
            string s = "List of Processes Server";
            foreach (Process theprocess in processlist)
            {
                s += ("\nProcess: " + theprocess.ProcessName + " ID: " + theprocess.Id);
            }
            return s;
        }

        public static void KillProcess(int id)
        {
            ConsoleDisplay.Write("Killed process: " + id);
            //--- real command below ---
            //Process p = Process.GetProcessById(id);
            //p.Kill();
        }

        public static void KillProcesses(string name)
        {
            ConsoleDisplay.Write("Killed process: " + name);
            //--- real command below ---
            //Process[] p = Process.GetProcessesByName(name);
            //foreach (Process pp in p)
            //pp.Kill();
        }
    }
}
