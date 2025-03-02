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
		public IActionResult Partial_Item_View()
		{
			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
			if (shoppingCart is not null)
			{
				return PartialView(shoppingCart.Items);
			}
			return PartialView(null);
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

        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");

            if (shoppingCart is not null)
            {
				shoppingCart.UpdateQuantity(id, quantity);
                HttpContext.Session.SetObjectAsJson("cart", shoppingCart);
				return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }

        [HttpPost]
		public IActionResult Delete(int id)
		{
			var code = new
			{
				Success = false,
				msg = string.Empty,
				code = -1,
				count = 0
			};

			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");

			if (shoppingCart is not null)
			{
				var checkProduct = shoppingCart.Items.FirstOrDefault(x => x.ProductId == id);
				if (checkProduct is not null)
				{
					shoppingCart.Remove(id);

					code = new
					{
						Success = true,
						msg = string.Empty,
						code = 1,
						count = shoppingCart.Items.Count
					};

					HttpContext.Session.SetObjectAsJson("cart", shoppingCart);

					return Json(code);
				}
			}
			return Json(code);
		}

		[HttpPost]
		public IActionResult DeleteAll()
		{
			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
			if (shoppingCart is not null)
			{
				shoppingCart.ClearCart();
				HttpContext.Session.SetObjectAsJson("cart", shoppingCart);
				return Json(new { Success = true });
			}
			return Json(new { Success = false });
		}
	}
}
