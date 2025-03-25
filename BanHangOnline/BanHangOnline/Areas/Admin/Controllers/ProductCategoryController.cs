using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly WebStoreDbContext _db;
        public ProductCategoryController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            var items = _db.ProductCategory.ToList();
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Add(ProductCategory model)
		{
            if (ModelState.IsValid)
            {
				model.CreateDate = DateTime.Now;
				model.ModifierDate = DateTime.Now;

				model.Alias = Filter.FilterChar(model.Title);
				_db.ProductCategory.Add(model);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Edit(int id)
		{
			var model = _db.ProductCategory.FirstOrDefault(x => x.Id == id);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ProductCategory model)
		{
			if (ModelState.IsValid)
			{
				model.ModifierDate = DateTime.Now;
				model.Alias = Filter.FilterChar(model.Title);
				_db.ProductCategory.Update(model);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var item = _db.ProductCategory.FirstOrDefault(x => x.Id == id);
			if (item is not null)
			{
				_db.ProductCategory.Remove(item);
				_db.SaveChanges();
				return Json(new { success = true });
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
						var obj = _db.ProductCategory.FirstOrDefault(x => x.Id == int.Parse(item));
						if (obj is not null)
						{
							_db.ProductCategory.Remove(obj);
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
