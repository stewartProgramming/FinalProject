using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class MatchResultViewModel
    {
        public class FootballMatches
        {
            public string name { get; set; }
            public List<Match> matches { get; set; }
        }

        public class Match
        {
            public string round { get; set; }
            public string group { get; set; }
            public string date { get; set; }
            public string team1 { get; set; }
            public string team2 { get; set; }
            public Score score { get; set; }
        }

        public class Score
        {
            public int[] ft { get; set; }
            public int[] et { get; set; }
        }


    }
}
