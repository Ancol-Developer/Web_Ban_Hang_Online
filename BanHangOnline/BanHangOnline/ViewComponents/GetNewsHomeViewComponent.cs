using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.ViewComponents
{
    public class GetNewsHomeViewComponent : ViewComponent
    {
        private readonly WebStoreDbContext _db;
        public GetNewsHomeViewComponent(WebStoreDbContext db)
        {
            this._db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _db.News.Take(3).ToListAsync();
            return View(items);
        }
    }
}
