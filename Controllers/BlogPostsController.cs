using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Controllers
{
    [Authorize]
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogPostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BlogPosts (shows the current user’s blog posts)
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var posts = _context.BlogPosts.Include(b => b.Category)
                                          .Where(b => b.ApplicationUserId == userId);
            return View(await posts.ToListAsync());
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPost)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                blogPost.PublishDate = DateTime.Now;
                blogPost.ApplicationUserId = userId;
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(blogPost);
        }


        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();
            if (blogPost.ApplicationUserId != _userManager.GetUserId(User))
                return Forbid();
            ViewBag.Categories = _context.Categories.ToList();
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost)
        {
            if (id != blogPost.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await _context.BlogPosts.FindAsync(id);
                    if (existingPost.ApplicationUserId != _userManager.GetUserId(User))
                        return Forbid();

                    existingPost.Title = blogPost.Title;
                    existingPost.Content = blogPost.Content;
                    existingPost.CategoryId = blogPost.CategoryId;
                    existingPost.ImageUrl = blogPost.ImageUrl;
                    // Do not change PublishDate
                    _context.Update(existingPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BlogPosts.Any(e => e.Id == blogPost.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts
                                .Include(b => b.Category)
                                .FirstOrDefaultAsync(b => b.Id == id);
            if (blogPost == null) return NotFound();
            if (blogPost.ApplicationUserId != _userManager.GetUserId(User))
                return Forbid();
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost.ApplicationUserId != _userManager.GetUserId(User))
                return Forbid();
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
