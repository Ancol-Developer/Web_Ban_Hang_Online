﻿using Microsoft.AspNetCore.Mvc;

namespace BanHangOnline.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
