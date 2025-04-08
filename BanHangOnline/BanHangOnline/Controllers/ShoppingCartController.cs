using BanHangOnline.Common;
using BanHangOnline.Models;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
	{
		private readonly WebStoreDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ShoppingCartController(WebStoreDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			this._db = db;
			this._webHostEnvironment = webHostEnvironment;
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

		public IActionResult CheckOut()
		{
			ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
			if (shoppingCart is not null)
			{
				ViewBag.CheckCart = shoppingCart.Items;
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CheckOut(OrderViewModel orderRq)
		{
			var code = new
			{
				Success = false,
				Code = -1,
			};

			if (ModelState.IsValid)
			{
				ShoppingCart? shoppingCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("cart");
				if (shoppingCart is not null)
				{
					Order order = new Order();
					order.CustomerName = orderRq.CustomerName;
					order.Phone = orderRq.Phone;
					order.Address = orderRq.Address;
					order.Email = orderRq.Email;

					shoppingCart.Items.ForEach(x => order.OrderDetail?.Add(new OrderDetail
					{
						ProductId = x.ProductId,
						Quantity = x.Quantity,
						Price = x.Price,
					}));

					order.TotalAmount = shoppingCart.Items.Sum(x => x.Price * x.Quantity);
					order.TypePayment = orderRq.TypePayment;
					order.Code = "DH" + Guid.NewGuid().ToString();
					order.CreateDate = DateTime.Now;
					order.ModifierDate = DateTime.Now;
					order.CreateBy = orderRq.Phone;

					_db.Order.Add(order);
					_db.SaveChanges();

					// Send Mail to Customer
					string contentCustomer = string.Empty;

					using (StreamReader reader = new StreamReader(Path.Combine(_webHostEnvironment.WebRootPath, "Templates", "send2.html")))
					{
						contentCustomer = reader.ReadToEnd();
					}

					string strProduct = string.Empty;

					foreach (var product in shoppingCart.Items)
					{
						strProduct += "<tr>";
						strProduct += "<td>" + product.ProductName + "</td>";
						strProduct += "<td>" + product.Quantity + "</td>";
						strProduct += "<td>" + Common.Common.FormatNumber(product.TotalPrice, 0) + "</td>";
						strProduct += "</tr>";
					}

					contentCustomer = contentCustomer.Replace("{{MaDon}}",order.Code);
					contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
					contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
					contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
					contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
					contentCustomer = contentCustomer.Replace("{{SanPham}}", strProduct);
					contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
					contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Common.Common.FormatNumber(order.TotalAmount, 0));
					contentCustomer = contentCustomer.Replace("{{TongTien}}", Common.Common.FormatNumber(order.TotalAmount, 0));

					Common.Common.SendMail("AncolShop", "Đơn hàng #" + order.Code, contentCustomer, orderRq.Email);

					// Update Session
					shoppingCart.Items.Clear();
					HttpContext.Session.SetObjectAsJson("cart", shoppingCart);

					return RedirectToAction("CheckOutSuccess", new { isSuccess = true });
				}
			}
			return RedirectToAction("CheckOutSuccess", new { isSuccess = false });
		}

		[HttpGet]
		public IActionResult CheckOutSuccess(bool isSuccess)
		{
			return View(isSuccess);
		}
	}
}
