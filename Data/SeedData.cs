using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using BlogApp.Models;

namespace BlogApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Stop if the DB has been seeded
                if (context.BlogPosts.Any())
                {
                    return;
                }

                // Seed Categories
                var techCategory = new Category { Name = "Technology" };
                var lifeCategory = new Category { Name = "Lifestyle" };
                context.Categories.AddRange(techCategory, lifeCategory);
                context.SaveChanges();

                // Create a test user (for demo purposes; in a real app use UserManager)
                var testUser = new ApplicationUser
                {
                    UserName = "testuser@example.com",
                    Email = "testuser@example.com"
                };
                context.Users.Add(testUser);
                context.SaveChanges();

                // Seed Blog Posts
                context.BlogPosts.AddRange(
                    new BlogPost
                    {
                        Title = "First Blog Post",
                        Content = "This is the content of the first blog post.",
                        PublishDate = DateTime.Now.AddDays(-10),
                        CategoryId = techCategory.Id,
                        ApplicationUserId = testUser.Id
                    },
                    new BlogPost
                    {
                        Title = "Second Blog Post",
                        Content = "This is the content of the second blog post.",
                        PublishDate = DateTime.Now.AddDays(-5),
                        CategoryId = lifeCategory.Id,
                        ApplicationUserId = testUser.Id
                    }
                );
                context.SaveChanges();

                // Seed a Comment on the first post
                var blogPost = context.BlogPosts.First();
                context.Comments.Add(new Comment
                {
                    Content = "Great post!",
                    PublishDate = DateTime.Now.AddDays(-9),
                    BlogPostId = blogPost.Id,
                    ApplicationUserId = testUser.Id
                });
                context.SaveChanges();
            }
        }
    }
}
