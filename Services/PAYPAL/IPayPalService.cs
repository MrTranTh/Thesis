using Thesis.Models.PAYPAL;

namespace Thesis.Services.PAYPAL
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
