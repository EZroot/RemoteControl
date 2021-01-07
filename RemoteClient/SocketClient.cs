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
    public class SocketClient
    {
        static bool running = true;
        static bool runningStream = true;
        static bool isStreaming = false;

        public static void Start()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5555);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine("Press any key to connect...");
                Console.ReadKey();
                Console.Clear();
                try
                {
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

                            Console.WriteLine(":> Currently streaming mouse position...");
                            while (runningStream)
                            {
                                Vector2 mousePos = RemoteCommand.GetMousePosition();

                                byte[] m = Encoding.ASCII.GetBytes("SENT: " + mousePos.X + "," + mousePos.Y + " <EOF>");
                                //byte[] m = Encoding.ASCII.GetBytes("mm " + mousePos.X + "," + mousePos.Y + " <EOF>");
                                int bsent = sender.Send(m);
                                //listen for response?
                                int brec = sender.Receive(bytes); //get status from server if we recieved a function call, if doesnt exist or something return null/custom error code
                            }
                        }
                        else
                        {
                            Console.Write(":>");
                            string input = Console.ReadLine() + " <EOF>";
                            //exit
                            if (input == "exit" || input == "quit")
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
