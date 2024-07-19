using System.Collections.Concurrent;

namespace Thesis.MiddleWare
{
    public class ThongKeTruyCapMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, bool> _onlineUsers;

        public ThongKeTruyCapMiddleware(RequestDelegate next)
        {
            _next = next;
            _onlineUsers = new ConcurrentDictionary<string, bool>();
        }

        public async Task Invoke(HttpContext context)
        {
            string userId = GetUserIdFromCookie(context);

            if (!string.IsNullOrEmpty(userId) && !context.Request.Path.StartsWithSegments("/admin"))
            {
                _onlineUsers.TryAdd(userId, true);
            }

            context.Items["OnlineUsersCount"] = _onlineUsers.Count;

            await _next(context);
        }

        private string GetUserIdFromCookie(HttpContext context)
        {
            string userId = context.Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString(); // Tạo một userId mới nếu không có trong cookie
                context.Response.Cookies.Append("UserId", userId);
            }

            return userId;
        }
    }

    public static class ThongKeTruyCapMiddlewareExtensions
    {
        public static IApplicationBuilder ThongKeTruyCapMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThongKeTruyCapMiddleware>();
        }
    }
}


