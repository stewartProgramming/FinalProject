using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class FavoriteTeamViewModel
    {
        public string round { get; set; }
        public string group { get; set; }
        public string date { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public Score score { get; set; }
    }

    public class ScoreFavoriteTeamViewModel
    {
        public int[] ft { get; set; }
        public int[] et { get; set; }
    }
}
