using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class UserPredictions
    {
        public string UserId { get; set; }
        public int? Points { get; set; }
        public string MatchDay { get; set; }
        public string MatchDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string MatchPick { get; set; }
        public string MatchResult { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
