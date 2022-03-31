using System;
using System.Threading;
using System.Net.Sockets;

namespace Client
{
    class clnt
    {
        static void Main(string[] args)
        {
            string myMessage = "start";
            TcpClient client = null;
            client = Connect("127.0.0.1");

            while (myMessage != "-1")
            {
                Console.WriteLine("\n Enter a message...");
                myMessage = Console.ReadLine();

                if (myMessage.CompareTo("recieve") == 0)
                {
                    recieveMessage(client);
                }
                else
                {
                    SendMessage(client, myMessage + "$");
                }
            }
            if (client.Connected)
            {
                client.Close();
            }
        }

        static TcpClient Connect(String server)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                return client;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                return null;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                return null;
            }

        }

        static void SendMessage(TcpClient cl, String message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            NetworkStream stream = cl.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            data = new Byte[256];

            String responseData = String.Empty;

            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
        }

        static void recieveMessage(TcpClient cl)
        {
            string message = "";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            NetworkStream stream = cl.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            data = new Byte[256];

            String responseData = String.Empty;

            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
        }
    }
}
