using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{

    public class FootballStandings
    {
        public string get { get; set; }
        public Parameters parameters { get; set; }
        public object[] errors { get; set; }
        public int results { get; set; }
        public Paging paging { get; set; }
        public Response[] response { get; set; }
    }

    public class Parameters
    {
        public string league { get; set; }
        public string season { get; set; }
    }

    public class Paging
    {
        public int current { get; set; }
        public int total { get; set; }
    }

    public class Response
    {
        public League league { get; set; }
    }

    public class League
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string logo { get; set; }
        public string flag { get; set; }
        public int season { get; set; }
        public Standing[][] standings { get; set; }
    }

    public class Standing
    {
        public int rank { get; set; }
        public Team team { get; set; }
        public int points { get; set; }
        public int goalsDiff { get; set; }
        public string group { get; set; }
        public string form { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public All all { get; set; }
        public Home home { get; set; }
        public Away away { get; set; }
        public DateTime update { get; set; }
    }

    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }

    public class All
    {
        public int played { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lose { get; set; }
        public Goals goals { get; set; }
    }

    public class Goals
    {
        public int _for { get; set; }
        public int against { get; set; }
    }

    public class Home
    {
        public int played { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lose { get; set; }
        public Goals1 goals { get; set; }
    }

    public class Goals1
    {
        public int _for { get; set; }
        public int against { get; set; }
    }

    public class Away
    {
        public int played { get; set; }
        public int win { get; set; }
        public int draw { get; set; }
        public int lose { get; set; }
        public Goals2 goals { get; set; }
    }

    public class Goals2
    {
        public int _for { get; set; }
        public int against { get; set; }
    }

}
