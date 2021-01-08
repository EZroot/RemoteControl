using RemoteControl.Controller;
using RemoteControl.Display;
using RemoteControl.Executables.Windows;
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
        //dotnet publish -c release -r ubuntu.16.04-x64 --self-contained
        public static void Start()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            //IPAddress ipAddress = IPAddress.Parse("138.197.144.107");
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
                            //need to parse data recieved
                            //if string equals keyword, run function
                            ConsoleDisplay.Write("Command recieved: " + data.ToString());
                            data = CommandReader.ReadCommands(data);

                            //echos msg back to client
                            byte[] msg = Encoding.ASCII.GetBytes(data);
                            clientHandler.Send(msg);
                            data = "";
                        }
                    }
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
