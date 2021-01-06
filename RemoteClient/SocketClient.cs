using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    byte[] msg = Encoding.ASCII.GetBytes("/mm 555,335 <EOF>"); //we made our own file reader by specifying eof, we can do other stuff like specific function, or set a variable

                    int bytesSent = sender.Send(msg);

                    //listen for response?
                    int bytesRec = sender.Receive(bytes); //get status from server if we recieved a function call, if doesnt exist or something return null/custom error code
                    Console.WriteLine("Bytes sent: " + bytesSent.ToString());
                    Console.WriteLine("Bytes recieved: " + bytesRec.ToString());
                    Console.WriteLine("Sent string: {0}", Encoding.ASCII.GetString(msg, 0, bytesSent));
                    Console.WriteLine("raw bytes: ");
                    for (int i = 0; i < msg.Length; i++)
                    {
                        Console.Write(msg[i]);
                    }
                    Console.WriteLine("Recieved string in bytes: ");
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        Console.Write(bytes[i]);
                    }
                    Console.WriteLine("Recieved string: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
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
