using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    public class NewsController : Controller
    {
        private readonly WebStoreDbContext _db;

        public NewsController(WebStoreDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
