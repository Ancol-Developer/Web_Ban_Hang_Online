using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductImagesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly WebStoreDbContext _db;
        public ProductImagesController(WebStoreDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _db = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int id)
        {
            var items = _db.ProductImage.Where(x => x.ProductId == id).ToList();
            ViewBag.ProductId = items.First().ProductId;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(int productId, string url)
        {
            await _db.ProductImage.AddAsync(new ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false,
            });

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _db.ProductImage.FirstOrDefault(x => x.Id == id);
            if (item is not null
                && item.Image is not null)
            {
                string relativePath = item.Image;
                if (relativePath.StartsWith("/"))
                {
                    relativePath = relativePath.Substring(1);
                }
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
                if (System.IO.File.Exists(uploadDir))
                {
                    System.IO.File.Delete(uploadDir);
                }

                _db.ProductImage.Remove(item);
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> images, int productId = -9999, bool isUpdateDatabase = false)
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
                    string pathImage = "/uploads/" + uniqueFileName;
                    uploadedImagePaths.Add(pathImage);

                    // Update to database
                    if (isUpdateDatabase)
                    {
                        _db.ProductImage.Add(new ProductImage
                        {
                            ProductId = productId,
                            Image = pathImage,
                            IsDefault = false,
                        });
                    }
                }
                await _db.SaveChangesAsync();
            }

            return Json(uploadedImagePaths);
        }
    }
}
