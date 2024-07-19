using Microsoft.AspNetCore.Mvc;

namespace Thesis.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
