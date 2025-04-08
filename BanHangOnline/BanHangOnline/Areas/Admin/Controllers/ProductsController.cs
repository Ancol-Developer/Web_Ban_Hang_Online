using BanHangOnline.Common;
using Entities;
using Entities.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class ProductsController : Controller
    {
        private readonly WebStoreDbContext _db;
        public ProductsController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index(string searchString, int? pageNumber)
        {
            int pageSize = 10;
            var items = _db.Product.Include(x => x.ProductCategory).Include(x => x.ProductImage).OrderByDescending(x => x.Id).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Alias?.Contains(searchString) == true || s.Title?.Contains(searchString) == true).ToList();
            }

            ViewBag.PageSize = pageSize;

            // Save Search string
            ViewBag.SearchText = searchString;
            return View(Common<Product>.CreateAsync(items, pageNumber ?? 1, pageSize));
        }

        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategory.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product model, List<string> listImages, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (listImages.Any() && listImages.Count > 0)
                {
                    for (int i = 0; i < listImages.Count; i++)
                    {
                        if (i + 1 == rDefault.FirstOrDefault())
                        {
                            model.Image = listImages[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = listImages[i],
                                IsDefault = true,
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = listImages[i],
                                IsDefault = false,
                            });
                        }
                    }
                }
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;

                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = Filter.FilterChar(model.Title ?? string.Empty);
                }

                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }

                model.ProductCategory = _db.ProductCategory.FirstOrDefault(x => x.Id == model.ProductCategoryId);

                _db.Product.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategory = new SelectList(_db.ProductCategory.ToList(), "Id", "Title");
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategory.ToList(), "Id", "Title");
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                _db.Product.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategory = new SelectList(_db.ProductCategory.ToList(), "Id", "Title");
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                _db.Product.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsActive = !item.IsActive;
                _db.Product.Update(item);
                _db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsHome(int id)
        {
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsHome = !item.IsHome;
                _db.Product.Update(item);
                _db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsHot(int id)
        {
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsHot = !item.IsHot;
                _db.Product.Update(item);
                _db.SaveChanges();
                return Json(new { success = true, isHot = item.IsHot });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsSale(int id)
        {
            var item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item is not null)
            {
                item.IsSale = !item.IsSale;
                _db.Product.Update(item);
                _db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }

            return Json(new { success = false });
        }
    }
}
