using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace JukeboxClient
{
    internal class MusicPlayer
    {
        private int songIndex = 0;
        private bool isPlaying = false;
        private MediaElement song;
        private String musicDirectory = Path.Combine(Directory.GetCurrentDirectory() + @"../../../../music");


        public MediaElement loadSong(MediaElement songPlayer)
        { 
            songPlayer.Source = new Uri(Directory.GetFiles(musicDirectory)[songIndex]);
            songPlayer.LoadedBehavior = MediaState.Manual;
            return songPlayer;
        }

        public void streamSong(MediaElement songPlayer)
        {
            if (songPlayer.Source != null)
            {
                songPlayer.Play();
                setPlaying(true);
            }
        }

        public void pauseSong(MediaElement songPlayer)
        {
            songPlayer.Pause();
            setPlaying(false);
        }

        public int incrementSong()
        {
            songIndex++;
            if (songIndex >= Directory.GetFiles(musicDirectory).Length) {
                songIndex = 0;
            }
            return songIndex;
        }

        public int decrementSong()
        {
            songIndex--;
            if (songIndex < 0)
            {
                songIndex = (Directory.GetFiles(musicDirectory).Length - 1);
            }
            return songIndex;
        }

        public bool getPlaying()
        {
            return isPlaying;
        }

        public void setPlaying(bool playing)
        {
            isPlaying = playing;
        }
    }
}
