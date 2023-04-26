using MessagePack;

namespace ShoppingCart.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }

        public bool? IsApproved { get; set; } = false;
    }
}

