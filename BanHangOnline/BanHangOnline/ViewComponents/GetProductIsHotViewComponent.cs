using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.ViewComponents
{
    public class GetProductIsHotViewComponent : ViewComponent
    {
        private readonly WebStoreDbContext _db;
        public GetProductIsHotViewComponent(WebStoreDbContext db)
        {
            this._db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _db.Product.Where(x => x.IsHot && x.IsActive).Take(12).ToListAsync();
            return View(items);
        }
    }
}
