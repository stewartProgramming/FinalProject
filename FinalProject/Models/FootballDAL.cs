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

        public static List<Club> GetTeams()
        {
            string data = CallTeamAPI();
            Rootobject r = JsonConvert.DeserializeObject<Rootobject>(data);
            List<Club> clubs = r.clubs.ToList();
            return clubs;
        }
    }
}
