using BanHangOnline.Common;
using BanHangOnline.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly WebStoreDbContext _db;

		public ShoppingCartController(WebStoreDbContext db)
		{
			this._db = db;
		}
		public IActionResult Index()
		{
			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
            if (shoppingCart is not null)
            {
				View(shoppingCart.Items);
			}
            return View();
		}

		[HttpGet]
		public IActionResult ShowCount()
		{
			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
            if (shoppingCart is not null)
            {
				return Json(new
				{
					count = shoppingCart.Items.Count,
				});
			}
            return Json(new
			{
				count = 0,
			});
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(int id, int quantity)
		{
			var code = new
			{
				Success = false,
				msg = string.Empty,
				code = -1,
				count = 0
			};

			var checkProduct = await _db.Product.Include(x => x.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
			if (checkProduct is not null)
			{
				ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");

				if (shoppingCart is null)
				{
					shoppingCart = new ShoppingCart();
				}

				ShoppingCartItem item = new ShoppingCartItem()
				{
					ProductId = checkProduct.Id,
					ProductName = checkProduct.Title,
					CategoryName = checkProduct.ProductCategory?.Title,
					ProductImage = checkProduct.Image,
					Alias = checkProduct.Alias,
					Quantity = quantity,
				};

				// Price
				item.Price = checkProduct.Price;
				if (checkProduct.PriceSale > 0)
				{
					item.Price = (decimal)checkProduct.PriceSale;
				}

				// Total Price
				item.TotalPrice = item.Price * item.Quantity;

				shoppingCart.AddToCard(item, quantity);

				HttpContext.Session.SetObjectAsJson("cart", shoppingCart);
				code = new
				{
					Success = true,
					msg = "Thêm sản phẩm vào giỏ hàng thành công!",
					code = 1,
					count = shoppingCart.Items.Count,
				};
			}

			return Json(code);
		}
	}
}
