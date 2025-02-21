using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Controllers
{
	public class ProductController : Controller
	{
		private readonly WebStoreDbContext _db;

		public ProductController(WebStoreDbContext db)
        {
			this._db = db;
		}

        public async Task<IActionResult> Index(int? id)
		{
			var items = await _db.Product.Include(x => x.ProductImage).ToListAsync();
			if (id is not null)
			{
				items = await _db.Product.Include(x => x.ProductImage).Where(x => x.Id == id).ToListAsync();
			}
			return View(items);
		}

		public async Task<IActionResult> ProductCategory(string alias, int id)
		{
			var items = await _db.Product.Include(x => x.ProductImage).ToListAsync();
			if (id > 0)
			{
				items = await _db.Product.Include(x => x.ProductImage).Include(x => x.ProductCategory).Where(x => x.ProductCategoryId == id).ToListAsync();
			}

			var cate = _db.ProductCategory.FirstOrDefault(x => x.Id == id);

			if (cate is not null)
			{
				ViewBag.CateName = cate.Title;
			}
			ViewBag.CateId = id;
			return View(items);
		}

		public IActionResult Detail(string alias, int id)
		{
			var item = _db.Product.Include(x => x.ProductCategory).FirstOrDefault(x => x.Id == id);
			return View(item);
		}
	}
}
