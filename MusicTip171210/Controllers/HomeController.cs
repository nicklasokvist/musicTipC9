using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicTip171210.Models;
using MusicTip171210.SpotifyApiHandler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
/// Null or Empty checks need to be added to methods (some errors).
/// Error handling to be added to methods (Try-Catch-blocks, some errors in http responses).
/// Json-parse to be moved to Api Handler.
/// Static globals used for testing - to be stored as session data
/// /Nicklas 171211
/// </summary>
namespace MusicTip171210.Controllers
{
    public class HomeController : Controller
    {
        // static globals for testing - should probany be stored as session data
        private static MusicTipViewModel musicTipViewModel = new MusicTipViewModel();
        private static SpotifyApiClient client = new SpotifyApiClient();
        private static Artist artist;
        private static List<SelectionSeed> selectionSeedReference = new List<SelectionSeed>();

        /// <summary>
        /// Return View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(musicTipViewModel);
        }

        /// <summary>
        /// Get Artists from Spotify, update ViewModel, return view
        /// </summary>
        /// <param name="serachArtist"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SearchArtist(string serachArtist)
        {
            ResetSelectInputs();

            serachArtist = Request.Form["SearchArtist"];
            SearchArtistResponse searchArtistResponse = client.SearchArtistsAsync(serachArtist).Result;
            var artistObjects = searchArtistResponse.Artists.Items;

            List<SelectListItem> artistsListSelectListItems = new List<SelectListItem>();

            foreach (Artist artist in artistObjects)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = artist.Name,
                    Value = artist.Id
                };
                artistsListSelectListItems.Add(selectList);

                IEnumerable<SelectListItem> elist = artistsListSelectListItems;
                musicTipViewModel.Artists = elist;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Select artist as selection seed.
        /// Update ViewModel with genres.
        /// Update ViewModel with Top-Tracks (from Spotify API)
        /// </summary>
        /// <param name="selectedArtist"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SelectArtist(string selectedArtist)
        {
            selectedArtist = Request.Form["SelectArtist"];
            IEnumerable<string> artists = Request.Form["Artists"];
            string artistId = artists.First();
            artist = client.GetArtistAsync(artistId).Result;

            AddSelectionSeed("Artist", artistId, artist.Name);

            UpdateGenres(artist.Genres);

            UpdateTopTracks(artistId);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Add track as selection seed.
        /// Track name sould be piced up from local table for improved performance. /Nicklas 171211
        /// </summary>
        /// <param name="selectedTrack"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SelectTrack(string selectedTrack)
        {
            IEnumerable<string> tracks = Request.Form["Tracks"];
            string trackId = tracks.First();

            string result = client.GetTrackAsync(trackId).Result;
            string trackName = JObject.Parse(result)["name"].ToString();

            AddSelectionSeed("Track", trackId, trackName);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Add genre as selection seed.
        /// </summary>
        /// <param name="selectedGenre"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SelectGenre(string selectedGenre)
        {
            IEnumerable<string> genres = Request.Form["Genres"];
            string genreId = genres.First();

            AddSelectionSeed("Genre", genreId, genreId);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add SelectionSeed, call collection handler, get recommendations
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private void AddSelectionSeed(string type, string id, string name)
        {
            SelectionSeed selectionSeed = new SelectionSeed(type, id, name);

            selectionSeedReference.Add(selectionSeed);

            SelectonSeedCollectionHandle();

            GetRedommendations();
        }
        /// <summary>
        /// Build parameters, make recommendation request and update ViewModel.
        /// JSON parse - handling should be done in Api handler class /Nicklas 171211
        /// </summary>
        private void GetRedommendations()
        {
            // Build parameters for request
            string seed_artists = "";
            string seed_genres = "";
            string seed_tracks = "";
            foreach (SelectionSeed s in selectionSeedReference)
            {
                //System.Diagnostics.Debug.WriteLine(s.Type);
                //System.Diagnostics.Debug.WriteLine(s.Name);
                //System.Diagnostics.Debug.WriteLine(s.Id);
                switch (s.Type)
                {
                    case "Artist":
                        if (string.IsNullOrEmpty(seed_artists))
                            seed_artists = s.Id;
                        else
                            seed_artists = seed_artists + "," + s.Id;
                        break;
                    case "Genre":
                        if (string.IsNullOrEmpty(seed_genres))
                            seed_genres = s.Id;
                        else
                            seed_genres = seed_genres + "," + s.Id;
                        break;
                    case "Track":
                        if (string.IsNullOrEmpty(seed_tracks))
                            seed_tracks = s.Id;
                        else
                            seed_tracks = seed_tracks + "," + s.Id;
                        break;
                }

            }
            // get redommendations
            string redommendationResults = client.GetRecommendationsAsync(seed_artists, seed_genres, seed_tracks).Result;

            // handle Json parse (code landed here cause of parse error... - to be moved to api handler class) 
            JObject a = JObject.Parse(redommendationResults);
            IList<JToken> b = a["tracks"].Children().ToList();

            // Update ViewModel
            List<SelectListItem> redommendationTracks = new List<SelectListItem>();
            foreach (object result in b)
            {
                //System.Diagnostics.Debug.WriteLine(result);
                //System.Diagnostics.Debug.WriteLine(JObject.Parse(result.ToString())["name"]);
                //System.Diagnostics.Debug.WriteLine(JObject.Parse(result.ToString())["id"]);

                SelectListItem selectList = new SelectListItem()
                {
                    Text =  JObject.Parse(result.ToString())["album"]["artists"][0]["name"].ToString() + " - " +
                            JObject.Parse(result.ToString())["name"].ToString(),
                    Value = JObject.Parse(result.ToString())["id"].ToString()
                };
                redommendationTracks.Add(selectList);
            }
            IEnumerable<SelectListItem> elist = redommendationTracks;
            musicTipViewModel.Recommendations = elist;

        }
        /// <summary>
        /// Reset Artists, Genres and Top Tracks selects.
        /// </summary>
        private void ResetSelectInputs()
        {
            musicTipViewModel.Artists = null;
            musicTipViewModel.Genres = null;
            musicTipViewModel.Tracks = null;
        }
        /// <summary>
        /// Update ViewModel Genres
        /// </summary>
        /// <param name="genreObjects"></param>
        private void UpdateGenres(IList<object> genreObjects)
        {
            List<SelectListItem> genreListSelectListItems = new List<SelectListItem>();
            foreach (string genre in genreObjects)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = genre,
                    Value = genre
                };
                genreListSelectListItems.Add(selectList);
            }
            musicTipViewModel.Genres = genreListSelectListItems;
        }
        /// <summary>
        /// Update ViewModel Top Tracks
        /// JSON parse - handling should be done in Api handler class /Nicklas 171211
        /// </summary>
        /// <param name="artistId"></param>
        private void UpdateTopTracks(string artistId)
        {
            List<SelectListItem> artistsTopTracks = new List<SelectListItem>();
            string x = client.GetTopTracksAsync(artistId, "SE").Result;
            JObject a = JObject.Parse(x);
            IList<JToken> b = a["tracks"].Children().ToList();
            foreach (object result in b)
            {
                System.Diagnostics.Debug.WriteLine(result);
                System.Diagnostics.Debug.WriteLine(JObject.Parse(result.ToString())["name"]);
                System.Diagnostics.Debug.WriteLine(JObject.Parse(result.ToString())["id"]);

                SelectListItem selectList = new SelectListItem()
                {
                    Text = JObject.Parse(result.ToString())["name"].ToString(),
                    Value = JObject.Parse(result.ToString())["id"].ToString()
                };
                artistsTopTracks.Add(selectList);
            }
            IEnumerable<SelectListItem> elist = artistsTopTracks;
            musicTipViewModel.Tracks = elist;
        }
        /// <summary>
        /// Max 5 selection seeds can be used in API-call, limit SelectoinSeed collection size.
        /// </summary>
        private void SelectonSeedCollectionHandle()
        {
            List<SelectListItem> selectionSeedList = new List<SelectListItem>();
            // Remove oldest post
            while (selectionSeedReference.Count() > 5)
            {
                selectionSeedReference.RemoveAt(0);
            }
            // Update ViewModel
            foreach (SelectionSeed s in selectionSeedReference)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id
                };
                selectionSeedList.Add(selectListItem);
            }
            musicTipViewModel.SelectionSeeds = selectionSeedList;
        }
    }

}