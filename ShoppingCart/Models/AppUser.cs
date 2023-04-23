using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Models
{
    public class AppUser:IdentityUser
    {
       public string? Occupation { get; set; }

        public bool IsAproved { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();
    }
}
