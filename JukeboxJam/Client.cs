using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class JukeboxClient
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:80/");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}

