using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly WebStoreDbContext _db;
        public CategoryController(WebStoreDbContext dbContext)
        {
            _db= dbContext;
        }

        // GET: Admin/Category
        public IActionResult Index()
        {
            var items = _db.Category.ToList();
            return View(items);
        }

        // GET: Admin/Category/Add
        public IActionResult Add()
        {
            return View();
        }

        // Post: Admin/Category/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            return View();
        }
    }
}
