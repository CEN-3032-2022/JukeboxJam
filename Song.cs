using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JukeboxClient
{
    public class Song
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; } = String.Empty;
        [JsonProperty("artist")]
        public string Artist { get; set; } = String.Empty;
        [JsonProperty("album")]
        public string Album { get; set; } = String.Empty;
    }
}
