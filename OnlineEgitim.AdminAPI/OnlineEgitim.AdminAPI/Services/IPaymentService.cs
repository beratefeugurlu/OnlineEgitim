using OnlineEgitim.AdminAPI.Models;

namespace OnlineEgitim.AdminAPI.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
    }
}
