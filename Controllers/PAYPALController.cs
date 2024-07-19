using Microsoft.AspNetCore.Mvc;
using Thesis.Models.PAYPAL;
using Thesis.Services.PAYPAL;

namespace Thesis.Controllers
{
    public class PAYPALController : Controller
    {
        private readonly IPayPalService _payPalService;

        public PAYPALController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = await _payPalService.CreatePaymentUrl(model, HttpContext);
            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            var response = _payPalService.PaymentExecute(Request.Query);

            return View(response);
        }
    }
}
