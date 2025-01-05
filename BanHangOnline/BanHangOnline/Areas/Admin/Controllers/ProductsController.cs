using BanHangOnline.Common;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly WebStoreDbContext _db;
        public ProductsController(WebStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index(string searchString, int? pageNumber)
        {
            int pageSize = 10;
            var items = _db.Product.OrderByDescending(x => x.Id).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Alias?.Contains(searchString) == true || s.Title?.Contains(searchString) == true).ToList();
            }

            ViewBag.PageSize = pageSize;

            // Save Search string
            ViewBag.SearchText = searchString;
            return View(PaginatedList<Product>.CreateAsync(items, pageNumber ?? 1, pageSize));
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
