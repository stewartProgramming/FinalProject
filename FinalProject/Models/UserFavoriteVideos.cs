using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class UserFavoriteVideos
    {
        public int VideoId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual CommunityFavoriteVideos Video { get; set; }
    }
}
