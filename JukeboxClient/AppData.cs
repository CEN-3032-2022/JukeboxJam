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

        static AppData()
        {
            playlist = new List<Song>();
        }
    }
}
