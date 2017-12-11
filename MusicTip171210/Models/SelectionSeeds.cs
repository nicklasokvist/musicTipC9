using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTip171210.Models
{
    /// <summary>
    /// Seed used for getting recommendations from Spotify.
    /// Types Artist, Genre, Track, AddedTime intended for removal function (turned out to not be needed, can be removed). /Nicklas 171211
    /// </summary>
    public class SelectionSeed
    {
        public SelectionSeed() {  }
        public SelectionSeed(string type, string id, string name)
        {
            Type = type;
            Id = id;
            Name = name;
        }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        DateTime AddedTime { get; set; }
    }
}
