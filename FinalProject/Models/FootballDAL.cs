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
        public static string CallTeamAPI(string league)
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/2020-21/{league}.clubs.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static string CallStandingsAPI(string league)
        {
            string url = $"https://api-football-beta.p.rapidapi.com/standings?season=2020&league={league}";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers.Add("x-rapidapi-key", "8ee6bb1f97msh72388712e57f0eep108ff5jsnb7ec224de70c");
            request.Headers.Add("x-rapidapi-host", "api-football-beta.p.rapidapi.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static FootballStandings GetStandings(string league)
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

            string data = CallStandingsAPI(league);
            FootballStandings s = JsonConvert.DeserializeObject<FootballStandings>(data);
            return s;
        }

        public static FootballClubs GetTeams(string league)
        {
            string data = CallTeamAPI(league);
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
            string url = $"https://www.scorebat.com/video-api/v1/";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static List<Highlight> GetHighlights()
        {
            string data = CallHighlightAPI();
            List<Highlight> r = JsonConvert.DeserializeObject<List<Highlight>>(data);
            return r;
        }

        //public static FootballMatches GetMatches(string league)
        //{
        //    string data = CallMatchAPI(league);
        //    FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
        //    FootballMatches matches = r;
        //    return matches;
        //}

        public static List<Match> GetMatches(string league, string season)
        {
            string data = CallMatchAPI(league, season);
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            List<Match> matches = r.matches.ToList();
            return matches;
        }
    }
}
