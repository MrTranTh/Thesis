using Microsoft.AspNetCore.Mvc;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TruyCapController : Controller
    {
        private readonly AccessLogService _accessLogService;

        public TruyCapController(AccessLogService accessLogService)
        {
            _accessLogService = accessLogService;
        }

        public IActionResult Index()
        {
            ViewBag.OnlineUsersCount = HttpContext.Items["OnlineUsersCount"];
            _accessLogService.LogAccess();
            var accessCountByDay = _accessLogService.GetAccessCountByDay();
            var accessCountByMonth = _accessLogService.GetAccessCountByMonth();
            var accessCountByYear = _accessLogService.GetAccessCountByYear();

            var viewModel = new ThongKeViewModel
            {
                AccessCountByDay = accessCountByDay,
                AccessCountByMonth = accessCountByMonth,
                AccessCountByYear = accessCountByYear
            };

            return View(viewModel);
        }
    }
}
