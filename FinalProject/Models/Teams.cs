using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class Teams
    {
        public Teams()
        {
            UserFavoriteTeams = new HashSet<UserFavoriteTeams>();
        }
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int? LeagueId { get; set; }

        public virtual Leagues League { get; set; }
        public virtual ICollection<UserFavoriteTeams> UserFavoriteTeams { get; set; }
    }
}
