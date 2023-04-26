using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using ShoppingCart.Data;

namespace ShoppingCart.Repository
{
    public class TaxRateRepository : ITaxRateRepository
    {
        private readonly DataContext _context;

        public TaxRateRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TaxRate> GetLatestTaxRateAsync()
        {
            return await _context.TaxRates
                .OrderByDescending(tr => tr.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddTaxRateAsync(TaxRate taxRate)
        {
            await _context.TaxRates.AddAsync(taxRate);
            await _context.SaveChangesAsync();
        }
    }
}
