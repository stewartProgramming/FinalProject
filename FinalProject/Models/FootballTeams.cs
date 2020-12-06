using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class FootballClubs
    {
        public string name { get; set; }
        public List<Club> clubs { get; set; }
    }

    public class Club
    {
        public string name { get; set; }
        public string code { get; set; }
        public string country { get; set; }
    }
}
