using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;
using System.Linq;
using System.Threading.Tasks;


namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Home/Index
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Start with the base query including related entities
            var postsQuery = _context.BlogPosts
                                     .Include(b => b.Category)
                                     .Include(b => b.ApplicationUser)
                                     .AsQueryable();

            // Apply filtering if a categoryId is provided
            if (categoryId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // Retrieve the list of categories for the view
            ViewBag.Categories = await _context.Categories.ToListAsync();

            // Execute the query and pass the results to the view
            var posts = await postsQuery.ToListAsync();
            return View(posts);
        }


        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts
                                .Include(b => b.Category)
                                .Include(b => b.ApplicationUser)
                                .Include(b => b.Comments)
                                    .ThenInclude(c => c.ApplicationUser)
                                .FirstOrDefaultAsync(b => b.Id == id);
            if (blogPost == null) return NotFound();
            return View(blogPost);
        }
    }
}
