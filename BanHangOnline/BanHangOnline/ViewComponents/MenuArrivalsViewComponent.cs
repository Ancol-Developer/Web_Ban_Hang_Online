﻿using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangOnline.ViewComponents
{
	public class MenuArrivalsViewComponent : ViewComponent
	{
		private readonly WebStoreDbContext _db;
		public MenuArrivalsViewComponent(WebStoreDbContext db)
		{
			this._db = db;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = await _db.ProductCategory.ToListAsync();
			return View(items);
		}
	}
}
