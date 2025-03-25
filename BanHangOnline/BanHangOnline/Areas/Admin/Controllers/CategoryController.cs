using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

        public IActionResult Edit(int id)
        {
            var items = _db.Category.FirstOrDefault(x => x.Id == id);
            return View(items);  
        }

        // Post: Admin/Category/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = Filter.FilterChar(model.Title ?? string.Empty);
                _db.Category.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Category.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                _db.Category.Remove(item);
                _db.SaveChanges();
                return Json(new {success = true});
            }

            return Json(new { success = false });
        }
    }
}
