using RemoteControl.Display;
using RemoteControl.Executables.Windows;
using RemoteControl.Parser;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

namespace RemoteControl.Controller
{
    class CommandReader
    {
        public static string ReadCommands(string data)
        {
            if (CommandParser.Parse(data, "help") || CommandParser.Parse(data, "h"))
            {
                //read thru dictionary of commands and display them after we switch all this crap over
                data = "h - help\nmc - mouse click\nmm X,Y - move mouse\nms - start mouse stream\nyt url - opens and plays youtube website\nb url - opens url in browser\np - process list";
            }
            else if (CommandParser.Parse(data, "mouseclick") || CommandParser.Parse(data, "mc"))
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
                //Thread newT = new Thread(StreamMouse);
                
                //newT.Start();
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
                    ConsoleDisplay.Write("Failed: " + ex.Message + "\nTry /b url");
                }
            }
            else if(CommandParser.Parse(data,"kill") || CommandParser.Parse(data,"k"))
            {
                try
                {
                    string key = CommandParser.ParseStrings(data)[1];
                    int id = CommandParser.ParseNumbers(data)[0];
                    if (id==0) //there was no number in the command so it mustve been a process name
                        RemoteCommand.KillProcesses(key); //string version
                    else
                        RemoteCommand.KillProcess(id); //id version
                }
                catch (Exception ex)
                {
                    ConsoleDisplay.Write("Failed: " + ex.Message + "\nTry /k id or /k name");
                }
            }
            else if (CommandParser.Parse(data,"processes") || CommandParser.Parse(data,"p"))
            {
                try
                {
                    data = RemoteCommand.ListProcesses();
                }
                catch(Exception e)
                {
                    ConsoleDisplay.Write("Failed: " + e.Message);
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
