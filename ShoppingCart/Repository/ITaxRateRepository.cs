using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public interface ITaxRateRepository
    {
        Task<TaxRate> GetLatestTaxRateAsync();
        Task AddTaxRateAsync(TaxRate taxRate);
    }
}
