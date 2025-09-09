using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface IPaymentMethods
    {
        Task<ApplicationResponse<List<PaymentMethod>>> GetAllPaymentMethodsAsync();
        Task<ApplicationResponse<PaymentMethod>> GetPaymentMethodByIdAsync(Guid id);
        Task<ApplicationResponse<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod);
        Task<ApplicationResponse<PaymentMethod>> UpdatePaymentMethodAsync(Guid id, PaymentMethod paymentMethod);
        Task<ApplicationResponse<PaymentMethod>> DeletePaymentMethodAsync(Guid id);
    }
}