using BanHangOnline.Common;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace BanHangOnline.Areas.Admin.Controllers
{
	public class OrderController : Controller
	{
		private readonly WebStoreDbContext _db;
        public OrderController(WebStoreDbContext db)
        {
			_db = db;
		}
        public IActionResult Index(int? page)
		{
			var items = _db.Order.OrderByDescending(x => x.CreateDate).ToList();
			if (page is null)
			{
				page = 1;
			}

			int pageSize = 10;
			return View(Common<Order>.CreateAsync(items, page ?? 1, pageSize));
		}
	}
}
