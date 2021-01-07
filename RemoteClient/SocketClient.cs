using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteClient
{
    public class SocketClient
    {
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

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    bool running = true;
                    while (running)
                    {
                        Console.Write(":>");
                        string input = Console.ReadLine() + "<EOF>";

                        //exit
                        if (input == "exit" || input == "quit")
                        {
                            Console.WriteLine("\nExiting client...");
                            Thread.Sleep(5);
                            running = false;
                            break;
                        }

                        byte[] msg = Encoding.ASCII.GetBytes(input); //we made our own file reader by specifying eof, we can do other stuff like specific function, or set a variable

                        int bytesSent = sender.Send(msg);

                        //listen for response?
                        int bytesRec = sender.Receive(bytes); //get status from server if we recieved a function call, if doesnt exist or something return null/custom error code

                        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));
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
    }
}
