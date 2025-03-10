using System.Collections.Concurrent;

namespace BanHangOnline.Models
{
	public class UserSessionTracker
	{
		private static ConcurrentDictionary<string, DateTime> ActiveSessions = new();

		public static void AddUser(string sessionId)
		{
			ActiveSessions[sessionId] = DateTime.UtcNow;
		}

		public static void RemoveUser(string sessionId)
		{
			ActiveSessions.TryRemove(sessionId, out _);
		}

		public static int GetOnlineUserCount()
		{
			return ActiveSessions.Count;
		}
	}
}
