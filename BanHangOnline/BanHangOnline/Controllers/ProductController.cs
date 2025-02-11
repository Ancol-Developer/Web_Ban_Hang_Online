using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Controllers
{
	public class ProductController : Controller
	{
		private readonly WebStoreDbContext _db;

		public ProductController(WebStoreDbContext db)
        {
			this._db = db;
		}

        public IActionResult Index()
		{
			return View();
		}
	}
}
