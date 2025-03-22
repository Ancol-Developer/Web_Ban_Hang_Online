using BanHangOnline.Models;
using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly WebStoreDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, WebStoreDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var items = _dbContext.Users.ToList();
            return View(items);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index");
                }
                AddError(result);
            }
            return View(model);
        }

        private void AddError(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
