
namespace ShoppingCart.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; } = new Category();
        public IEnumerable<Category> categories { get; set; } = new List<Category>();
    }
}
