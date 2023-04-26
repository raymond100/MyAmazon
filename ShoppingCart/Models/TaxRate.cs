namespace ShoppingCart.Models
{
    public class TaxRate
    {
        public int Id { get; set; }
        public decimal Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}