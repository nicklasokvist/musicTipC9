using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// All Spotify Api Classes collectd in one file. Recommended to be separated into muliptle files. /Nicklas 171211
/// </summary>
namespace MusicTip171210.Models
{

    public class Artist
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("genres")]
        public IList<object> Genres { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public IList<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class AuthenticationResponse
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }
    public class AvailableGenres
    {

        [JsonProperty("genres")]
        public IList<string> genres { get; set; }
    }
    public class ExternalUrls
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
    public class Image
    {

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
    public class SearchArtistCollection
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public IList<Artist> Items { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
    public class SearchArtistResponse
    {
        [JsonProperty("artists")]
        public SearchArtistCollection Artists { get; set; }
    }
    public class SpotifyUser
    {

        [JsonProperty("display_name")]
        public object display_name { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls external_urls { get; set; }

        //[JsonProperty("followers")]
        //public Followers followers { get; set; }

        [JsonProperty("href")]
        public string href { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("images")]
        public IList<object> images { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("uri")]
        public string uri { get; set; }
    }
    public class TopTracksResponse
    {
        public IList<Track> tracks { get; set; }

    }
    public class Track
    {
        [JsonProperty("album")]
        public string album { get; set; }

        [JsonProperty("artists")]
        public IList<Artist> artists { get; set; }

        [JsonProperty("available_markets")]
        public IList<string> available_markets { get; set; }

        [JsonProperty("disc_number")]
        public int disc_number { get; set; }

        [JsonProperty("duration_ms")]
        public int duration_ms { get; set; }

        [JsonProperty("explicit")]
        public bool _explicit { get; set; }

        [JsonProperty("external_ids")]
        public string external_ids { get; set; }

        [JsonProperty("external_urls")]
        public string external_urls { get; set; }

        [JsonProperty("href")]
        public string href { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("is_playable")]
        public bool is_playable { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("popularity")]
        public int popularity { get; set; }

        [JsonProperty("preview_url")]
        public object preview_url { get; set; }

        [JsonProperty("track_number")]
        public int track_number { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("uri")]
        public string uri { get; set; }
    }
}
