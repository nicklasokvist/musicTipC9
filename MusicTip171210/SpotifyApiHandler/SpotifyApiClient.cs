using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;
using MusicTip171210.Models;

/// <summary>
/// Class from example project modified and added to. My account used for testing.
/// Some trouble with parsing json that should be handeled here are handeld in HomeController. /Nicklas 171211
/// </summary>
namespace MusicTip171210.SpotifyApiHandler
{
    public class SpotifyApiClient
    {
        //private const string ClientId = "996d0037680544c987287a9b0470fdbb";
        //private const string ClientSecret = "5a3c92099a324b8f9e45d77e919fec13";

        // Nicklas as Client
        private const string ClientId = "ede555b342214faebe86dbcf66ba60f8";
        private const string ClientSecret = "d8e37f8ddd684402ad1109046f16972f";

        protected const string BaseUrl = "https://api.spotify.com/";
        private HttpClient GetDefaultClient()
        //public HttpClient GetDefaultClient(string clientId, string clientSecret)
        {
            var authHandler = new SpotifyAuthClientCredentialsHttpMessageHandler(
                ClientId,
                ClientSecret,
                new HttpClientHandler());

            var client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return client;
        }

        public async Task<SearchArtistResponse> SearchArtistsAsync(string artistName, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/search");
            url = url.SetQueryParam("q", artistName);
            url = url.SetQueryParam("type", "artist");

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            var artistResponse = JsonConvert.DeserializeObject<SearchArtistResponse>(response);
            return artistResponse;
        }

        public async Task<AvailableGenres> AvailableGenresAsync(int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/recommendations/available-genre-seeds");

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            var availableGenresResponse = JsonConvert.DeserializeObject<AvailableGenres>(response);
            return availableGenresResponse;
        }

        public async Task<SpotifyUser> SpotifyUserAsync(string spotifyUserId, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/users/" + spotifyUserId + "");

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            var spotifyUserResponse = JsonConvert.DeserializeObject<SpotifyUser>(response);
            return spotifyUserResponse;
        }

        public async Task<Artist> GetArtistAsync(string artistId, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/artists/" + artistId + "");

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            var artist = JsonConvert.DeserializeObject<Artist>(response);
            return artist;
        }

        //public async Task<TopTracksResponse> GetTopTracksAsync(string artistId, string country, int? limit = null, int? offset = null)
        public async Task<string> GetTopTracksAsync(string artistId, string country, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/artists/" + artistId + "/top-tracks");

            url = url.SetQueryParam("country", country);
            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            //TopTracksResponse topTracks = JsonConvert.DeserializeObject<TopTracksResponse>(response);

            //return topTracks;

            return response;
        }

        //public async Task<Track> GetTrackAsync(string trackId, int? limit = null, int? offset = null)
        public async Task<string> GetTrackAsync(string trackId, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/tracks/" + trackId);

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            //var track = JsonConvert.DeserializeObject<Track>(response);
            //return track;
            return response;
        }

        public async Task<string> GetRecommendationsAsync(string artists, string genres, string tracks, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/recommendations/");
            url = url.SetQueryParam("seed_artists", artists);
            url = url.SetQueryParam("seed_genres", genres);
            url = url.SetQueryParam("seed_tracks", tracks);

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            //var track = JsonConvert.DeserializeObject<Track>(response);
            //return track;
            return response;
        }


    }
}
