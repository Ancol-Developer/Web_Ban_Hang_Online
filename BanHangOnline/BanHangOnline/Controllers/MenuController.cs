using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
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
    }
}
