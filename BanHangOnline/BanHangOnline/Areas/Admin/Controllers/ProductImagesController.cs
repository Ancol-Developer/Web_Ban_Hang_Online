using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View(items);
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
