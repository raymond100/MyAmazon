using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Repository.BankSystem.BankSystemModels
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        public string NameOnCard { get; set; }
        public long CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int CVV { get; set; }
        public decimal IntialAmount { get; set; }
        public decimal CurrentAmount { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}
