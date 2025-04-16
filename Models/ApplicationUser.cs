using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation properties for related blog posts and comments
        public virtual ICollection<BlogPost> BlogPosts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
