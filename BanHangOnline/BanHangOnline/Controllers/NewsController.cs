using BanHangOnline.Common;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
    public class NewsController : Controller
    {
        private readonly WebStoreDbContext _db;

        public NewsController(WebStoreDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index(int? pageNumber)
        {
            var items = _db.News.ToList();
            int pageSize = 10;
            return View(Common<News>.CreateAsync(items, pageNumber ?? 1, pageSize));
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
