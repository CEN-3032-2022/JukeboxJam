using System.Text.Json;

public class Client
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    static readonly HttpClient client = new HttpClient();

    static async Task Main()
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try
        {
            //HttpResponseMessage response = await client.GetAsync("https://localhost:7078/getSongs");
            //response.EnsureSuccessStatusCode();
            //string responseString = response.Content.ReadAsStringAsync().Result;

            //Playlist p1 = JsonSerializer.Deserialize<Playlist>(responseString);

            //Console.WriteLine(responseString);

            


            //byte[] responseBody = await response.Content.ReadAsByteArrayAsync();

            //string path = @"C:\Users\jesse\source\repos\TestHttp\TestHttp\";
            //// Write file to song folder
            //using (FileStream fs = File.Create("ukulele.mp3"))
            //{
            //    fs.Write(responseBody);
            //}

            //// Above three lines can be replaced with new helper method below
            //// string responseBody = await client.GetStringAsync(uri);

            //Console.WriteLine(responseBody);
            Console.ReadKey();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}
