using BanHangOnline.Common;
using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly WebStoreDbContext _db;
        public NewsController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 8;
            
            return View(await PaginatedList<News>.CreateAsync(_db.News.OrderByDescending(x => x.Id), pageNumber ?? 1, pageSize));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;

                model.CategoryId = 2;

                model.Alias = Filter.FilterChar(model.Title);
                _db.News.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = _db.News.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = Filter.FilterChar(model.Title);
                _db.News.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.News.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                _db.News.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.News.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsActive = !item.IsActive;
                _db.News.Update(item);
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
                        var obj = _db.News.FirstOrDefault(x => x.Id == int.Parse(item));
                        if (obj is not null)
                        {
                            _db.News.Remove(obj);
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
