﻿using RemoteControl.Controller;
using RemoteControl.Display;
using RemoteControl.Parser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace RemoteControl.Server
{
    public class SocketServer
    {
        public static void Start()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndpoint = new IPEndPoint(ipAddress, 5555);

            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndpoint);
                listener.Listen(10);
                ConsoleDisplay.Write("Listening for connection...");

                while (true)
                {
                    Socket clientHandler = listener.Accept();
                    ConsoleDisplay.Write("Accepting client");

                    string data = null;
                    byte[] bytes = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = clientHandler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > 1)
                        {
                            break;
                        }
                    }

                    //need to parse data recieved
                    //if string equals keyword, run function
                    ConsoleDisplay.Write("Text recieved: " + data.ToString());

                    if (CommandParser.Parse(data, "mouseclick") || CommandParser.Parse(data, "mc"))
                    {
                        RemoteCommand.MouseClick();
                        ConsoleDisplay.Write("Left clicked mouse!");
                    }

                    if (CommandParser.Parse(data, "movemouse") || CommandParser.Parse(data, "mm"))
                    {
                        //ConsoleDisplay.Write("Pos: " + pos[0] +"/"+ pos[1]);
                        //Vector2 vec = new Vector2(pos[0], pos[1]);
                        //RemoteCommand.MoveMouse((int)vec.X, (int)vec.Y);

                        int[] derp = CommandParser.ParseNumbers(data.ToString());
                        RemoteCommand.MoveMouse(derp[0], derp[1]);
                        //ConsoleDisplay.Write("Moved mouse to X: " + vec.X + ", Y:" + vec.Y);
                    }

                    //echos msg back to client
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    clientHandler.Send(msg);
                    clientHandler.Shutdown(SocketShutdown.Both);
                    clientHandler.Close();
                }
            }
            catch (SocketException ex)
            {
                ConsoleDisplay.Write("Socket failed Error: " + ex.Message);
            }

            ConsoleDisplay.Write("End of server...");
            Console.ReadKey();
        }
    }
}
