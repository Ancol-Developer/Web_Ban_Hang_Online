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
	}
}
