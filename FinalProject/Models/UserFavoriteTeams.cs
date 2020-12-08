using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class UserFavoriteTeams
    {
        public string UserId { get; set; }
        public int? TeamId { get; set; }

        public virtual Teams Team { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
