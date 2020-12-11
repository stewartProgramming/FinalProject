using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class FootballDAL
    {
        private static DateTime _timeStamp = DateTime.Now;
        //private static List<Highlight> _cachedHighlights = new List<Highlight>();
        private IMemoryCache _cache;

        public FootballDAL(IMemoryCache cache)
        {
            _cache = cache;
        }

        public static string CallTeamAPI(string league, string season)
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/{season}/{league}.clubs.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static string CallStandingsAPI(string league, string season)
        {
            string url = $"https://api-football-beta.p.rapidapi.com/standings?season={season}&league={league}";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers.Add("x-rapidapi-key", "8ee6bb1f97msh72388712e57f0eep108ff5jsnb7ec224de70c");
            request.Headers.Add("x-rapidapi-host", "api-football-beta.p.rapidapi.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public FootballStandings GetStandings(string league, string season)
        {
            if (league == "en.1")
            {
                league = "39";
            }
            else if (league == "es.1")
            {
                league = "140";
            }
            else if (league == "de.1")
            {
                league = "78";
            }
            else if (league == "it.1")
            {
                league = "135";
            }
            else if (league == "fr.1")
            {
                league = "61";
            }

            if (season == "2019-20")
            {
                season = "2019";
            }
            else if (season == "2020-21")
            {
                season = "2020";
            }

            //creat unique key for each option; "{league} ¬_¬ {season}
            var cacheKey = string.Join("¬_¬", league, season);

            //check cache for key. If there is a match, return it
            if (_cache.TryGetValue(cacheKey, out FootballStandings cacheStandings)) { return cacheStandings; }
            
            //if cache doesn't have a match, go to api
            string data = CallStandingsAPI(league, season);
            FootballStandings s = JsonConvert.DeserializeObject<FootballStandings>(data);

            //put key and standings in cache for 15 min
            _cache.Set(cacheKey, s, new DateTimeOffset(DateTime.Now.AddMinutes(15)));

            return s;
        }

        public static FootballClubs GetTeams(string league, string season)
        {
            string data = CallTeamAPI(league, season);
            FootballClubs r = JsonConvert.DeserializeObject<FootballClubs>(data);
            return r;
        }

        public static string CallMatchAPI(string league, string season)
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/{season}/{league}.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static string CallHighlightAPI()
        {
            try
            {
                string url = $"https://www.scorebat.com/video-api/v1/";
                HttpWebRequest request = WebRequest.CreateHttp(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader rd = new StreamReader(response.GetResponseStream());
                string output = rd.ReadToEnd();
                return output;
            }
            catch
            {
                return string.Empty;
            }
        }

        public List<Highlight> GetHighlights()
        {
            //create key, doesn't need to be unique
            var cacheKey = "highlights";

            //if there is anything in cache for highlights, return it
            if (_cache.TryGetValue(cacheKey, out List<Highlight> cacheHighlights)) { return cacheHighlights; }

            string data = CallHighlightAPI();

            //if api fails, you get an empty string, this changes that to an empty list to controller
            if (string.IsNullOrWhiteSpace(data)) return new List<Highlight>();

            List<Highlight> r = JsonConvert.DeserializeObject<List<Highlight>>(data);

            //put key and results in cache for 15 min
            _cache.Set(cacheKey, r, new DateTimeOffset(DateTime.Now.AddMinutes(15)));
            return r;
        }

        public static FootballMatches GetMatches(string league, string season)
        {
            string data = CallMatchAPI(league, season);
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            FootballMatches matches = r;
            return matches;
        }

        public static List<Match> GetMatchesList(string league, string season)
        {
            string data = CallMatchAPI(league, season);
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            List<Match> matches = r.matches.ToList();
            return matches;
        }
    }
}
