using RemoteClient.Controller;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;

namespace RemoteClient
{
    //dotnet publish -c release -r ubuntu.16.04-x64 --self-contained

    public class SocketClient
    {
        static bool running = true;
        static bool runningStream = true;
        static bool isStreaming = false;

        public static void Start()
        {
            byte[] bytes = new byte[1024];

            Console.Write("Enter host: ");
            string hostEntry = Console.ReadLine();
            try
            {
                IPHostEntry host = Dns.GetHostEntry(hostEntry);
                IPAddress ipAddress = host.AddressList[0];
                //IPAddress ipAddress = IPAddress.Parse("138.197.144.107");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5555);
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    Console.WriteLine("Trying to connect to "+remoteEP.Address);
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    while (running)
                    {
                        //streaming mouse
                        if (isStreaming)
                        {
                            //press the anykey to exit mouse stream
                            Thread newThread = new Thread(BreakStream);
                            newThread.Start();

                            //Console.WriteLine("User@" + host.HostName + ": Currently streaming mouse position...");
                            while (runningStream)
                            {
                                Vector2 mousePos = RemoteCommand.GetMousePosition();

                               // byte[] m = Encoding.ASCII.GetBytes("SENT: " + mousePos.X + "," + mousePos.Y + " <EOF>"); //need to add EOF because were no longer adding it thru input
                                byte[] m = Encoding.ASCII.GetBytes("mm " + mousePos.X + "," + mousePos.Y + " <EOF>");
                                int bsent = sender.Send(m);
                                //listen for response?
                                int brec = sender.Receive(bytes); //get status from server if we recieved a function call, if doesnt exist or something return null/custom error code
                            }
                        }
                        else
                        {
                            string input = "";
                            while (input == "")
                            {
                                Console.Write("User@"+ipAddress.ToString()+":");
                                input = Console.ReadLine();
                            }
                            input += " <EOF>";

                            //exit
                            if (input.Contains("exit") || input.Contains("quit"))
                            {
                                Console.WriteLine("\nExiting client...");
                                Thread.Sleep(5);
                                running = false;
                                break;
                            }

                            if (input.Contains("ms") || input.Contains("mousestream"))
                            {
                                isStreaming = true;
                                runningStream = true;
                            }

                            byte[] msg = Encoding.ASCII.GetBytes(input); //we made our own file reader by specifying eof, we can do other stuff like specific function, or set a variable
                            int bytesSent = sender.Send(msg);
                            //listen for response?
                            int bytesRec = sender.Receive(bytes); //get status from server if we recieved a function call, if doesnt exist or something return null/custom error code
                            Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        }
                    }
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ex error: " + e.Message);
            }


            Console.WriteLine("Client exiting...");
            Console.ReadKey();
        }

        private static void BreakStream()
        {
            Console.ReadKey();

            isStreaming = false;
            runningStream = false;
        }
    }
}
