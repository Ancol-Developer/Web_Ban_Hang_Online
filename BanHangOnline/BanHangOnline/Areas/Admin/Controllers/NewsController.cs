using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly WebStoreDbContext _db;
        public NewsController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            var items = _db.News.OrderByDescending(x => x.Id).ToList();
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
