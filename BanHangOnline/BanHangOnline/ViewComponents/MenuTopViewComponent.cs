using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Components
{
	public class MenuTopViewComponent : ViewComponent
	{
		private readonly WebStoreDbContext _db;
		public MenuTopViewComponent(WebStoreDbContext db)
        {
			this._db = db;
		}
        public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = await _db.Category.OrderBy(x => x.Position).ToListAsync();
			return View(items);
		}
	}
}
