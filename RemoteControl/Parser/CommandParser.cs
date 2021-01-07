using RemoteControl.Display;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteControl.Parser
{
    public class CommandParser
    {
        public static bool Parse(string text, string command)
        {
            if (text.Contains(command))
            {
                return true;
            }
            return false;
        }

        public static string[] ParseStrings(string text)
        {
            ConsoleDisplay.Debug("Trying to parse: " + text);
            List<string> myStrings = new List<string>();
            string trim = Regex.Replace(text, @"s", "");

            Array.ForEach(trim.Split(null), s =>
            {
                myStrings.Add(s);
                ConsoleDisplay.Debug("Parsed String: " + s);
            });
            return myStrings.ToArray();
        }

        public static int[] ParseNumbers(string text)
        {
            ConsoleDisplay.Debug("Trying to parse: " + text);
            List<int> myIntegers = new List<int>();
            string trim = Regex.Replace(text, @"s", "");

            Array.ForEach(trim.Split(",".ToCharArray()), s =>
            {
                string resultString = Regex.Match(s, @"\d+").Value;
                int rs = Int32.Parse(resultString);
                myIntegers.Add(rs);
                ConsoleDisplay.Debug("Parsed Chunk: " + rs);
            });
            return myIntegers.ToArray();
        }
    }
}
