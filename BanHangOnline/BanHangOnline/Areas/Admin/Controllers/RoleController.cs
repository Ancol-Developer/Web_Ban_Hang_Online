using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var items = _roleManager.Roles;
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var item = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
