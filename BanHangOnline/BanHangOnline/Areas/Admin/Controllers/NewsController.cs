﻿using Entities;
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

        public IActionResult Index()
        {
            var items = _db.News.OrderByDescending(x => x.Id).ToList();
            return View(items);
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
    }
}
