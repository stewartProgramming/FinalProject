using System;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public partial class CommunityFavoriteVideos
    {
        public CommunityFavoriteVideos()
        {
            UserFavoriteVideos = new HashSet<UserFavoriteVideos>();
            VideoComments = new HashSet<VideoComments>();
        }

        public int Id { get; set; }
        public string EmbedCode { get; set; }
        public string VideoTitle { get; set; }
        public DateTime VideoDate { get; set; }

        public virtual ICollection<UserFavoriteVideos> UserFavoriteVideos { get; set; }
        public virtual ICollection<VideoComments> VideoComments { get; set; }
    }
}
