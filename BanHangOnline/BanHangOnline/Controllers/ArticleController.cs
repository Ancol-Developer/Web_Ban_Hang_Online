using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
    public class ArticleController : Controller
    {
        private readonly WebStoreDbContext _db;

        public ArticleController(WebStoreDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index(string alias)
        {
            var item = _db.Posts.FirstOrDefault(x => x.Alias == alias);
            return View(item);
        }
    }
}
