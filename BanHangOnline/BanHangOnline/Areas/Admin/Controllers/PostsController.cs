using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly WebStoreDbContext _db;
        public PostsController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            var items = _db.Posts.ToList();
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;

                model.CategoryId = 2;

                model.Alias = Filter.FilterChar(model.Title);
                _db.Posts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _db.Posts.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = Filter.FilterChar(model.Title);
                _db.Posts.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Posts.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                _db.Posts.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Posts.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsActive = !item.IsActive;
                _db.Posts.Update(item);
                _db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items is not null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _db.Posts.FirstOrDefault(x => x.Id == int.Parse(item));
                        if (obj is not null)
                        {
                            _db.Posts.Remove(obj);
                            _db.SaveChanges();
                        }
                    }
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
    }
}
