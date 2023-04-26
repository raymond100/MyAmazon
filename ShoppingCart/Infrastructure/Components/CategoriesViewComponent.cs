using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;

namespace ShoppingCart.Infrastructure.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly DataContext _context;

        public CategoriesViewComponent(DataContext context)
        {
            _context = context;
        }

        //public async Task<IViewComponentResult> InvokeAsync() => View(await _context.Categories.ToListAsync());

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.Categories.Where(c => c.IsApproved == true).ToListAsync());
    }
}



