using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Repository.BankSystem
{
    public interface IBank
    {
        Task<Status> VerifyPayment(PaymentData paymentData);
        Task<Status> ProceedPayment(PaymentData paymentData);

    }
}
