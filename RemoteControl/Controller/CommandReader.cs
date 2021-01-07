﻿using RemoteControl.Display;
using RemoteControl.Executables.Windows;
using RemoteControl.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteControl.Controller
{
    class CommandReader
    {
        public static string ReadCommands(string data)
        {
            if (CommandParser.Parse(data, "mouseclick") || CommandParser.Parse(data, "mc"))
            {
                RemoteCommand.MouseClick();
                ConsoleDisplay.Write("Left clicked mouse!");
            }
            else if (CommandParser.Parse(data, "movemouse") || CommandParser.Parse(data, "mm"))
            {
                int[] derp = CommandParser.ParseNumbers(data.ToString());
                try
                {
                    RemoteCommand.MoveMouse(derp[0], derp[1]);
                    ConsoleDisplay.Write("Moved mouse on client to X: " + derp[0] + " ,Y: " + derp[1]);
                }
                catch (Exception ex)
                {
                    ConsoleDisplay.Write("Failed: " + ex.Message + "\nTry /mm x,y");
                }
            }
            else if (CommandParser.Parse(data, "mousestream") || CommandParser.Parse(data, "ms"))
            {
                //while true, get clients mouse coords and stream it
            }
            else if (CommandParser.Parse(data, "youtube") || CommandParser.Parse(data, "yt"))
            {
                try
                {
                    string key = CommandParser.ParseStrings(data)[1];

                    if (CommandParser.Parse(key, "Close"))
                        Youtube.Close();
                    
                    Youtube.PlayVideo(key, true);
                }
                catch (Exception ex)
                {
                    ConsoleDisplay.Write("Failed: " + ex.Message +"\nTry /yt url");
                }
            }
            else if (CommandParser.Parse(data, "browser") || CommandParser.Parse(data, "b"))
            {
                try
                {
                    string key = CommandParser.ParseStrings(data)[1];

                    if (CommandParser.Parse(key, "Close"))
                        Browser.Close();

                    Browser.OpenUrl(key);
                }
                catch (Exception ex)
                {
                    ConsoleDisplay.Write("Failed: " + ex.Message + "\nTry /yt url");
                }
            }
            else
            {
                data = "Server: Command failed! Please use correct syntax. Type /help for list of commands. <EOF>";
            }
            return data;
        }
    }
}