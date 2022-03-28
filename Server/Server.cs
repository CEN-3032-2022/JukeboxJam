using System.Net;
using System.Net.Http;

public class Server {

    public static void Main(string[] args)
    {
        SimpleListenerExample();
    }

    // This example requires the System and System.Net namespaces.
    public static void SimpleListenerExample()
    {
        string[] prefixes = { "http://localhost:8080/" };

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        // URI prefixes are required,
        // for example "http://contoso.com:8080/index/".
        if (prefixes == null || prefixes.Length == 0)
            throw new ArgumentException("prefixes");

        // Create a listener.
        HttpListener listener = new HttpListener();

        // Add the prefixes.
        foreach (string s in prefixes)
        {
            listener.Prefixes.Add(s);
        }
        listener.Start();
        Console.WriteLine("Listening...");

        // Note: The GetContext method blocks while waiting for a request.
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;

        // Obtain a response object.
        HttpListenerResponse response = context.Response;

        // Construct a response.
        //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";

        //Read a file into the stream
        FileStream fileStream = File.OpenRead(@"C:\Users\jesse\Music\ukulele.mp3");
        byte[] sendBuffer = new byte[fileStream.Length];
        fileStream.Read(sendBuffer, 0, sendBuffer.Length);

        // Get a response stream and write the response to it.
        //response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(sendBuffer, 0, sendBuffer.Length);

        // You must close the output stream.
        output.Close();
        listener.Stop();
    }

}