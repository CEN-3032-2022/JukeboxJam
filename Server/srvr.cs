using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class srvr
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();

                List<handleClient> regClients = new List<handleClient>();

                TcpClient clnt = null;
                string message = null;

                // Enter the listening loop.
                while (true)
                {
                    for (int i = regClients.Count - 1; i >= 0; i--)
                    {
                        if (!regClients[i].getClient().Connected)
                        {
                            Console.WriteLine(i + " - disconnected");
                            regClients.RemoveAt(i);
                        }
                        if (null != (message = regClients[i].getNextMessage()) && message.Length > 0)
                        {
                            Console.WriteLine("proccess client (" + i + ") : '" + message + "'");

                            for (int k = 0; k < regClients.Count; k++)
                            {
                                regClients[k].sendMessage("server proccessed client (" + i + ") - '" + message + "'");
                            }
                        }
                    }

                    if (server.Pending())
                    {
                        Console.Write("Accepting a connection... ");
                        clnt = server.AcceptTcpClient();
                        if (clnt != null)
                        {
                            handleClient hc = new handleClient();
                            hc.startClient(clnt);
                            regClients.Add(hc);
                            Console.WriteLine("new connection!");
                        }
                        else
                        {
                            Console.Write("failed connection... ");
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
