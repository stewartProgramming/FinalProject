using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class VideoComments
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public string UserId { get; set; }
        public string VideoComment { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual CommunityFavoriteVideos Video { get; set; }
    }
}
