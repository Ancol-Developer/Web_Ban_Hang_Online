using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly WebStoreDbContext _db;
        public CategoryController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
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
        public async Task<IActionResult> Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.Alias = Filter.FilterChar(model.Title ?? string.Empty);
                await _db.Category.AddAsync(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
