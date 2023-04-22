using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Repository.BankSystem.BankSystemModels
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public long CardNumber { get; set; }
        public int TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public decimal TransactionValue { get; set; }

    }
}
