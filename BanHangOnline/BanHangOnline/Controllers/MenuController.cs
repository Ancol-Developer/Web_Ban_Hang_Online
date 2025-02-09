using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    public class MenuController : Controller
    {
        private readonly WebStoreDbContext _db;

        public MenuController(WebStoreDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MenuTop()
        {
            var items = _db.Category.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }
    }
}
