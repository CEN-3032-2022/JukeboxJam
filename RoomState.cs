using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JukeboxClient
{
    public class RoomState
    {
        [JsonProperty("position")]
        public TimeSpan Position { get; set; }
        [JsonProperty("songIndex")]
        public int SongIndex { get; set; }
        [JsonProperty("songState")]
        public int SongState { get; set; }
    }
}
