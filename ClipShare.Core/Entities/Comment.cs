using System;
using System.ComponentModel.DataAnnotations;

namespace ClipShare.Core.Entities
{
    public class Comment
    {
        public int AppUserId { get; set; }
        public int VideoId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        //Navigations
        public AppUser AppUser { get; set; }
        public Video Video { get; set; }
    }
}
