using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;



public class JukeboxClient
{
    static void Main()
    {
        InitializeConnection();       
    }

    private static void testAudio(Stream stream)
    {
        Console.Write("Testing audio...");
        StreamAudio jc = new StreamAudio(stream);
        jc.buttonPlay_Click();
        jc.timer1_Tick();
    }

    private static void InitializeConnection()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 1302);
            NetworkStream stream = client.GetStream();

            byte[] temp = new byte[1960595];
            stream.Read(temp, 0, temp.Length);
            Stream responseStream = new MemoryStream(temp);
            testAudio(responseStream);
            Console.WriteLine("Here");
            

            //string messageToSend = "Hello from client!";

            //int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
            //byte[] sendData = new byte[byteCount];
            //sendData = Encoding.ASCII.GetBytes(messageToSend);

            //stream.Write(sendData, 0, sendData.Length);
            //Console.WriteLine("Sending data to server...");

            stream.Close();
            client.Close();
            Console.ReadKey();
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

}

