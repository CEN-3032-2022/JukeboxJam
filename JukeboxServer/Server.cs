using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class JukeboxServer
{
    public static int Main(string[] args)
    {
        StartListening();
        return 0;
    }

    public static void StartListening()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 1302);
        listener.Start();

        while (true)
        {
            try
            {
                Console.WriteLine("Waiting for a connection.");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client has been accepted.");
                NetworkStream stream = client.GetStream();

                using(FileStream fileStream = File.OpenRead("bensound-sunny.mp3"))
                {
                    byte[] sendBuffer = new byte[fileStream.Length];
                    fileStream.Read(sendBuffer, 0, sendBuffer.Length);
                    Console.WriteLine("Sent audio data to client...");
                    stream.Write(sendBuffer, 0, sendBuffer.Length);
                }

                

                //// buffer used for receiving data
                //byte[] buffer = new byte[1024];
                //stream.Read(buffer, 0, buffer.Length);

                //// count the number of bytes in buffer
                //int bytesReceived = 0;
                //foreach (byte b in buffer)
                //{
                //    if (b != 0)
                //    {
                //        bytesReceived++;
                //    }
                //}

                //string request = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                //Console.WriteLine("Request received " + request);
                stream.Flush();
                Console.WriteLine("End");
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}