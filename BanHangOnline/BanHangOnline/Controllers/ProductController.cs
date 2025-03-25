using BanHangOnline.Common;
using BanHangOnline.Models;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
    public class ProductController : Controller
	{
		private readonly WebStoreDbContext _db;
		private readonly IConfiguration _configuration;

		public ProductController(WebStoreDbContext db, IConfiguration configuration)
        {
			this._db = db;
			this._configuration = configuration;
		}

        public async Task<IActionResult> Index(int? id)
		{
			var items = await _db.Product.Include(x => x.ProductImage).ToListAsync();
			if (id is not null)
			{
				items = await _db.Product.Include(x => x.ProductImage).Where(x => x.Id == id).ToListAsync();
			}

            var item = new ThongKeModel();
			var obj = new ThongKeTruyCap(_configuration);
			var data = obj.ThongKe();

			if (data is not null)
			{
				item.HomNay = data.HomNay.ToString("#,###");
				item.HomQua = data.HomQua.ToString("#,###");
				item.TuanNay = data.TuanNay.ToString("#,###");
				item.TuanTruoc = data.TuanTruoc.ToString("#,###");
				item.ThangNay = data.ThangNay.ToString("#,###");
				item.ThangTruoc = data.ThangTruoc.ToString("#,###");
				item.TatCa = data.TatCa.ToString("#,###");
			}

			ViewBag.visitor_online = UserSessionTracker.GetOnlineUserCount();

			ViewBag.DataView = item;

            return View(items);
		}

		public async Task<IActionResult> ProductCategory(string alias, int id)
		{
			var items = await _db.Product.Include(x => x.ProductImage).ToListAsync();
			if (id > 0)
			{
				items = await _db.Product.Include(x => x.ProductImage).Include(x => x.ProductCategory).Where(x => x.ProductCategoryId == id).ToListAsync();
			}

			var cate = _db.ProductCategory.FirstOrDefault(x => x.Id == id);

			if (cate is not null)
			{
				ViewBag.CateName = cate.Title;
			}
			ViewBag.CateId = id;
			return View(items);
		}

		public IActionResult Detail(string alias, int id)
		{
			var item = _db.Product.Include(x => x.ProductCategory).Include(x => x.ProductImage).FirstOrDefault(x => x.Id == id);
			if (item is not null)
			{
				item.ViewCount = item.ViewCount + 1;
				_db.Product.Update(item);
				_db.SaveChanges();
			}
			return View(item);
		}
	}
}
