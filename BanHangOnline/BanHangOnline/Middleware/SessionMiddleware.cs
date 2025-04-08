using BanHangOnline.Common;
using BanHangOnline.Models;

namespace BanHangOnline.Middleware
{
	public class SessionMiddleware
	{
		private readonly RequestDelegate _next;

		public SessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var sessionId = context.Session.Id;

			if (!context.Session.Keys.Contains("UserTracked"))
			{
				context.Session.SetObjectAsJson("UserTracked", "true");
				UserSessionTracker.AddUser(sessionId);
			}

			await _next(context);
		}

	}
}
