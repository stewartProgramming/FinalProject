using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class Teams
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int? LeagueId { get; set; }

        public virtual Leagues League { get; set; }
    }
}
