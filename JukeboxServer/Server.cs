using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class JukeboxServer
{
    public static int Main(string[] args)
    {
        StartServer();
        return 0;
    }

    public static void StartServer()
    {
        HttpListener listener = new HttpListener();
        HttpListenerContext context;
        HttpListenerRequest request;
        HttpListenerResponse response;
        Stream output;

        try
        {
            listener.Prefixes.Add("http://*:80/");
            listener.Start();

            // Note: The GetContext method blocks while waiting for a request.
            context = listener.GetContext();
            request = context.Request;
            response = context.Response;

            byte[] buffer = File.ReadAllBytes();

            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}