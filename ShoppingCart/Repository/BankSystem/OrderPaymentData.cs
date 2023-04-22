using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem.BankSystemModels;

namespace ShoppingCart.Repository.BankSystem
{
    public class OrderPaymentData
    {
        public Order Order { get; set; }
        public UserAccount Account { get; set; }
    }
}
