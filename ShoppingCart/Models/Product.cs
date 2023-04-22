using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Infrastructure.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please Enter a Value")]
        public string Name { get; set; }

        public string? Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum Length is 2")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please Enter a Value")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "You Must Choose a Category")]
        public long CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Category? Category { get; set; }
        public string Image { get; set; } = "noimage.png";
        
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }

        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }
        
    }
}

