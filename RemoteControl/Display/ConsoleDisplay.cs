using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Display
{
    public class ConsoleDisplay
    {
        public static void Write(string text)
        {
            //Console.SetCursorPosition(0, 0);
            Console.WriteLine(text);
            //Console.SetCursorPosition(0, 0);
        }

        public static void Write(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);
        }

        public static void Debug(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
