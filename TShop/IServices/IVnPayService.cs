using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPaymentRequestModel vnPaymentRequestModel);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collection);
    }
}
