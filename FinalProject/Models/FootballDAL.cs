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
        public static string CallTeamAPI()
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/2019-20/cl.clubs.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static string CallMatchAPI()
        {
            string url = $"https://raw.githubusercontent.com/openfootball/football.json/master/2019-20/cl.json";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            return output;
        }

        public static List<Club> GetTeams()
        {
            string data = CallTeamAPI();
            FootballClubs r = JsonConvert.DeserializeObject<FootballClubs>(data);
            List<Club> clubs = r.clubs.ToList();
            return clubs;
        }

        public static List<Match> GetMatches()
        {
            string data = CallMatchAPI();
            FootballMatches r = JsonConvert.DeserializeObject<FootballMatches>(data);
            List<Match> matches = r.matches.ToList();
            return matches;
        }
    }
}
