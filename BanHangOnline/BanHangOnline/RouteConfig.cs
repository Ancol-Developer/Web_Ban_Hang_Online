namespace BanHangOnline
{
	public static class RouteConfig
	{
		public static void RegisterRoutes(this WebApplication app)
		{
			app.MapAreaControllerRoute(
				name: "MyAreaAdmin",
				areaName: "Admin",
				pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "CategoryProduct",
				pattern: "san-pham/{id?}",
				new { controller = "Product", action = "index" });

			app.MapControllerRoute(
				name: "Products",
				pattern: "danh-muc-san-pham/{alias}-{id}",
				new { controller = "Product", action = "ProductCategory" });

			app.MapControllerRoute(
				name: "detail-Product",
				pattern: "chi-tiet/{alias}-p{id}",
				new { controller = "Product", action = "Detail" });

			app.MapControllerRoute(
				name: "Contact",
				pattern: "lien-he",
				new { controller = "Contact", action = "index" });

            app.MapControllerRoute(
                name: "ShoppingCart",
                pattern: "gio-hang",
                new { controller = "ShoppingCart", action = "index" });

			app.MapControllerRoute(
				name: "ShoppingCart",
				pattern: "thanh-toan",
				new { controller = "ShoppingCart", action = "CheckOut" });

            app.MapControllerRoute(
                name: "NewsList",
                pattern: "tin-tuc",
                new { controller = "News", action = "index" });

            app.MapControllerRoute(
               name: "NewsList",
               pattern: "n{id}",
               new { controller = "News", action = "Detail" });

            app.MapControllerRoute(
                name: "bài viết",
                pattern: "post/{alias?}",
                new { controller = "Article", action = "index" });
        }
	}
}
