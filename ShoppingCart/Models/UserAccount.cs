using ShoppingCart.Repository.BankSystem.BankSystemModels;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NameOnCard { get; set; }
        public long CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int CVV { get; set; }
        public PaymentType PaymentType { get; set; }
        
    
    }
}
