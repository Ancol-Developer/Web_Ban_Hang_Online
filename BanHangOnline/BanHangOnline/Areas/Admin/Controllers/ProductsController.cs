using BanHangOnline.Common;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly WebStoreDbContext _db;
        public ProductsController(WebStoreDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _db = dbContext;
            _webHostEnvironment = webHostEnvironment;
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
            ViewBag.ProductCategory = new SelectList(_db.ProductCategory.ToList(), "Id", "Title");
            return View();
        }

        #region Upload Image
        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> images)
        {
            var uploadedImagePaths = new List<string>();
            if (images is not null && images.Any())
            {
                foreach (var image in images)
                {
                    // Create directory save file
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Save file
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadDir, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Save directory to Data
                    uploadedImagePaths.Add("/uploads/" + uniqueFileName);
                }
            }

            return Json(uploadedImagePaths);
        }
        #endregion
    }
}
