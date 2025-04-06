using BanHangOnline.Models;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BanHangOnline.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly WebStoreDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, WebStoreDbContext db)
        {
            _logger = logger;
            this._db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subsrice(Subscribe subscribe)
        {
            if (ModelState.IsValid)
            {
                await _db.Subscribe.AddAsync(new Subscribe
                {
                    CreateDate = DateTime.Now,
                    Email = subscribe.Email,
                });
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thành công!";
            }
            else
            {
                TempData["SuccessMessage"] = null;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
