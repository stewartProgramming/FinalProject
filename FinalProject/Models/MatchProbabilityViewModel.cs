using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class MatchProbabilityViewModel
    {
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public double Team1Score { get; set; }
        public double Team2Score { get; set; }
    }
}
