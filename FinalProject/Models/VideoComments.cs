using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public partial class VideoComments
    {
        public int Id { get; set; }
        public int VideoId { get; set; }

        [ForeignKey("AspNetUsers")]
        public virtual string UserId { get; set; }

        [Display(Name = "Video Comment")]
        public string VideoComment { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual CommunityFavoriteVideos Video { get; set; }
    }
}