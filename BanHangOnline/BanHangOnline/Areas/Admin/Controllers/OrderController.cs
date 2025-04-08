using BanHangOnline.Common;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            ViewBag.PageSize = pageSize;

            return View(Common<Order>.CreateAsync(items, page ?? 1, pageSize));
		}

        public IActionResult DetailOrder(int id)
		{
			var item = _db.Order.Include(x => x.OrderDetail).ThenInclude(x => x.Product).FirstOrDefault(x => x.Id == id);
			return View(item);
		}

		[HttpPost]
		public IActionResult UpdateTT(int id, int state)
		{
            var item = _db.Order.FirstOrDefault(x => x.Id == id);
			if (item is not null)
			{
				item.TypePayment = state;
				_db.Order.Update(item);
				_db.SaveChanges();
				return Json(new { message= "Success", Success = true});
			}
            return Json(new { message = "Unsuccess", Success = false });
        }
	}
}
