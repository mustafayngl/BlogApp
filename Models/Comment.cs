using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        // Foreign Key to BlogPost
        public int BlogPostId { get; set; }
        public virtual BlogPost BlogPost { get; set; }

        // Foreign Key to the Comment Author (ApplicationUser)
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
