using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thesis.Models;

namespace Thesis.Services
{
    public class AccessLogService
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

 
        public void LogAccess()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var path = _httpContextAccessor.HttpContext.Request.Path;

            // Kiểm tra điều kiện trước khi ghi log truy cập
            if (!string.IsNullOrEmpty(userId) && !path.StartsWithSegments("/admin"))
            {
                var accessLog = new AccessLog
                {
                    AccessTime = DateTime.UtcNow
                };

                _context.AccessLogs.Add(accessLog);
                _context.SaveChanges();
            }

        }

        public Dictionary<DateTime, int> GetAccessCountByDay()
        {
            return _context.AccessLogs
                .AsEnumerable()
                .GroupBy(log => log.AccessTime.Date)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public Dictionary<string, int> GetAccessCountByMonth()
        {
            return _context.AccessLogs
                .AsEnumerable()
                .GroupBy(log => log.AccessTime.ToString("MM/yyyy"))
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public Dictionary<int, int> GetAccessCountByYear()
        {
            return _context.AccessLogs
                .AsEnumerable()
                .GroupBy(log => log.AccessTime.Year)
                .ToDictionary(group => group.Key, group => group.Count());
        }
    }

}
