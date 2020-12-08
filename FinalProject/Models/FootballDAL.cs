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

        public static List<Club> GetTeams(string league)
        {
            string data = CallTeamAPI(league);
            FootballClubs r = JsonConvert.DeserializeObject<FootballClubs>(data);
            List<Club> clubs = r.clubs.ToList();
            return clubs;
        }

        public static string CallMatchAPI()
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/2020-2021/en.1.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
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

        public static List<Match> GetMatches(string league)
        {
            string data = CallMatchAPI(league);
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            List<Match> matches = r.matches.ToList();
            return matches;
        }

        public static List<Match> GetMatches(string league, string season)
        {
            string data = CallMatchAPI(league, season);
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            List<Match> matches = r.matches.ToList();
            return matches;
        }
    }
}
