namespace BanHangOnline
{
	public static class RouteConfig
	{
		public static void RegisterRoutes(WebApplication app)
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
		}
	}
}
