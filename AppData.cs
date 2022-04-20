using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JukeboxClient
{
   /**
    * Stores the data of the application.
    */
    public static class AppData
    {
        public static List<Song> playlist;
        public static RoomState roomState;

        static AppData()
        {
            roomState = new RoomState();
            playlist = new List<Song>();
        }
    }
}
