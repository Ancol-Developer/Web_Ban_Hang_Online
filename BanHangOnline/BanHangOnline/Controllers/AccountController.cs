using BanHangOnline.Models;
using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebStoreDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, WebStoreDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._db = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // check for validation errors
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(temp => temp.ErrorMessage);
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
            }; ;

            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                // Sign in
                await _signInManager.SignInAsync(user, false);
                //return RedirectToAction(nameof(TradeController.Orders), "Trade");
            }
            else
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("Register", identityError.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction(nameof(TradeController.Orders), "Trade");
            }
            ModelState.AddModelError("Login", "Invalid email or password");
            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
