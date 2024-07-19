using Microsoft.AspNetCore.Mvc;
using Thesis.Models.VNPAY;
using Thesis.Services.VNPAY;

namespace Thesis.Controllers
{
    public class VNPAYController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public VNPAYController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            //return Json(response);
            return View(response);
        }
    }
}
