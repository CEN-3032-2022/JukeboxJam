﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jukebox.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private static List<Song> songs = new List<Song>
        {
            new Song
            {
                Id = 1,
                Title = "Acoustic Breeze",
                Artist = "Bensound",
                Album = "Generic",
                FileName = "acousticbreeze.mp3"
            },
            new Song
            {
                Id = 2,
                Title = "Ukulele",
                Artist = "Bensound",
                Album = "Generic",
                FileName = "ukulele.mp3"
            },
            new Song
            {
                Id = 3,
                Title = "All that",
                Artist = "Bensound",
                Album = "Generic",
                FileName = "allthat.mp3"
            },

        };

        [HttpGet("/getMp3/{id}")]
        public async Task<ActionResult<Song>> Get(int id)
        {
            // search for a song, can be modified for database instead
            var song = songs.Find(song => song.Id == id);
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

        [HttpGet("/getSongs")]
        public async Task<ActionResult<List<Song>>> Get()
        {
            Playlist p1 = new Playlist();
            p1.SongList = songs;

            return Ok(p1.SongList);
        }
    }
}