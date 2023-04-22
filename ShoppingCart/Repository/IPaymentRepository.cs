using Grpc.Core;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem;
using ShoppingCart.Repository.BankSystem.BankSystemModels;
using Status = ShoppingCart.Repository.BankSystem.BankSystemModels.Status;

namespace ShoppingCart.Repository
{
    public interface IPaymentRepository
    {
        public Status VendorPayment(VendorPaymentData paymentData);
        public Status OrderPayment(OrderPaymentData paymentData);
    }
}
