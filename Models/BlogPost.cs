using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        // Foreign key to Category
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        // Optional Image field
        // Allow null values:
        public string? ImageUrl { get; set; }

        // Foreign key to the Author (ApplicationUser)
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Navigation for comments
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
