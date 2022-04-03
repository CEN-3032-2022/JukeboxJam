using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jukebox.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        /**
         * Mock data for server. Will switch to database later.
         */
        private static List<Song> playlist = new List<Song>
        {
            new Song
            {
                Id = 1,
                Title = "Acoustic Breeze",
                Artist = "Bensound",
                Album = "Generic",
            },
            new Song
            {
                Id = 2,
                Title = "Ukulele",
                Artist = "Bensound",
                Album = "Generic",
            },
            new Song
            {
                Id = 3,
                Title = "All that",
                Artist = "Bensound",
                Album = "Generic",
            },

        };

        [HttpGet("/getMp3/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            // search for a song, can be modified for database instead
            var song = playlist.Find(song => song.Id == id);
            if (song == null)
                return BadRequest("Song not found!");

            string songPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = $"{song.Id}-{song.Title}-{song.Artist}.mp3".ToLower().Replace(" ", "%");
            songPath = Path.Combine(songPath, @"..\..\..\music", fileName);

            // read the file into a memory stream
            FileStream fileStream = System.IO.File.OpenRead(songPath);
            string mimeType = "audio/mpeg";

            // return the song to the client
            return new FileStreamResult(fileStream, mimeType)
            {
                // gives a name to the returned file
                FileDownloadName = fileName
            };        
        }

        [HttpGet("/getPlaylist")]
        public async Task<ActionResult<List<Song>>> Get()
        {
            return Ok(playlist);
        }
    }
}
