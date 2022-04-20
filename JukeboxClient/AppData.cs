using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<Song> playlist;

        static AppData()
        {
            playlist = new ObservableCollection<Song>();
        }
    }
}
