using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace JukeboxClient
{

    /**
    * This class is used to facilitate HTTP requests and responses.
    * It will also handle JSON serialization/deserialization.
    */
    public static class Network
    {
        private static HttpClient httpClient;
        private static string hostUrl = "http://localhost:5078/";

        static Network()
        {
            httpClient = new HttpClient();
        }

        /**
         * Obtains a playlist from the server,
         * Deserialize it and store into the program's
         * AppData.
         */

        public static void setHostUrl(string hUrl)
        {
            hostUrl = hUrl;
        }

        public static void GetPlaylist()
        {
            try
            {
                // construct request message
                string url = String.Concat(hostUrl, "getPlaylist");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                // obtain the response from the server
                var httpResponseMessage = httpClient.Send(request);
                var respStream = httpResponseMessage.Content.ReadAsStream();
                StreamReader reader = new StreamReader(respStream);
                string? jsonResponse = reader.ReadToEnd();
                Debug.WriteLine(jsonResponse);


                // deserialize the json string obtained from server
                List<Song>? songs = JsonConvert.DeserializeObject<List<Song>>(jsonResponse);

                // add the songs to the playlist
                if (songs != null)
                {
                    AppData.playlist.Clear();
                    foreach (Song song in songs)
                    {
                        AppData.playlist.Add(song);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /**
         * Takes a look at the music directory
         * and dynamically acquires missing songs
         * based off of the playlist received from
         * the server. If the playlist has a song
         * that is not saved in the music folder,
         * this method will acquire the missing songs.
         */
        public static async void GetSongs()
        {
            // setup the music path
            string songPath = AppDomain.CurrentDomain.BaseDirectory;
            songPath = Path.Combine(songPath, @"..\..\..\music");
            List<string> mp3Files = Directory.GetFiles(songPath).ToList();

            try
            {
                if (mp3Files.Count == 0)
                {
                    foreach (Song song in AppData.playlist)
                    {
                        string fileName = $"{song.Id}-{song.Title}-{song.Artist}.mp3".ToLower().Replace(" ", "%");

                        //obtain the mp3 from the server
                        string url = String.Concat(hostUrl, $"getMp3/{song.Id}");
                        var httpResponseMessage = await httpClient.GetAsync(url);
                        byte[] fileBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                        // save the file to the music folder
                        string newFilePath = Path.Combine(songPath, fileName);
                        using (FileStream fs = File.Create(newFilePath))
                        {
                            fs.Write(fileBytes);
                        }
                    }
                }
                else if (mp3Files.Count > 0)
                {
                    foreach (string mp3File in mp3Files)
                    {
                        /**
                        * Search the music directory for existing songs
                        * so we don't have to download them again.
                        */
                        foreach (Song song in AppData.playlist)
                        {
                            string fileName = $"{song.Id}-{song.Title}-{song.Artist}.mp3".ToLower().Replace(" ", "%");

                            if (!mp3File.Contains(fileName))
                            {
                                // obtain the mp3 from the server
                                string url = String.Concat(hostUrl, $"getMp3/{song.Id}");
                                var httpResponseMessage = await httpClient.GetAsync(url);
                                byte[] fileBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                                // save the file to the music folder
                                string newFilePath = Path.Combine(songPath, fileName);
                                using (FileStream fs = File.Create(newFilePath))
                                {
                                    fs.Write(fileBytes);
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task GetRoomState()
        {
            // obtain the response from the server
            string url = String.Concat(hostUrl, "getRoomState");
            var httpResponseMessage = await httpClient.GetAsync(url);
            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

            // deserialize the json string obtained from server
            AppData.roomState = JsonConvert.DeserializeObject<RoomState>(jsonResponse);
        }

        public static async Task PostRoomState()
        {
            string url = String.Concat(hostUrl, "postRoomState");
            var data = new StringContent(System.Text.Json.JsonSerializer.Serialize(AppData.roomState), Encoding.UTF8, "application/json");
            await httpClient.PostAsync(hostUrl, data);
        }
    }
}
