using ShoppingCart.Models;

namespace ShoppingCart.Repository.BankSystem.BankSystemModels
{
    public class PaymentData
    {
        public decimal Amount { get; set; }
        public UserAccount userAccount { get; set; }
    }
}
