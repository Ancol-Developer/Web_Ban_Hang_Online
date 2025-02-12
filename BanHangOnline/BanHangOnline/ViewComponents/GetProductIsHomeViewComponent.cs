using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.ViewComponents
{
	public class GetProductIsHomeViewComponent : ViewComponent
	{
		private readonly WebStoreDbContext _db;
		public GetProductIsHomeViewComponent(WebStoreDbContext db)
		{
			this._db = db;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = await _db.Product.Where(x => x.IsHome && x.IsActive).Take(12).ToListAsync();
			return View(items);
		}
	}
}
