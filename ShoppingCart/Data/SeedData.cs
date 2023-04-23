using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShoppingCart.Data
{
    public class SeedData
    {
        // private readonly  UserManager<AppUser> userManager;
        // private readonly  RoleManager<IdentityRole> roleManager;

        public static void SeedDatabase(DataContext context,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            // Add roles
            string[] roles = { "Admin", "Vendor", "Customer" };
            foreach (string role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = role
                    };
                    IdentityResult result = roleManager.CreateAsync(identityRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error occurred while creating role '{role}'.");
                    }
                }
            }

            // Add admin user
            string adminEmail = "admin@example.com";
            string adminPassword = "admin123";
            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                AppUser admin = new AppUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                IdentityResult result = userManager.CreateAsync(admin, adminPassword).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
                else
                {
                    throw new Exception($"Error occurred while creating admin user.");
                }
            }

            // Add vendors
            List<AppUser> vendors = new List<AppUser>
            {
                new AppUser
                {
                    Email = "vendor1@example.com",
                    UserName = "vendor1@example.com",
                    EmailConfirmed = true
                },
                new AppUser
                {
                    Email = "vendor2@example.com",
                    UserName = "vendor2@example.com",
                    EmailConfirmed = true
                }
            };
            foreach (AppUser vendor in vendors)
            {
                if (userManager.FindByEmailAsync(vendor.Email).Result == null)
                {
                    IdentityResult result = userManager.CreateAsync(vendor, "vendor123").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(vendor, "Vendor").Wait();
                    }
                    else
                    {
                        throw new Exception($"Error occurred while creating vendor user '{vendor.Email}'.");
                    }
                }
            }



            if (!context.Products.Any())
            {
                Category fruits = new Category { Name = "Fruits", Slug = "fruits" };
                Category shirts = new Category { Name = "Shirts", Slug = "shirts" };

                AppUser vendor1 = vendors.FirstOrDefault(u => u.Email == "vendor1@example.com");
                AppUser vendor2 = vendors.FirstOrDefault(u => u.Email == "vendor2@example.com");

                context.Products.AddRange(
                        new Product
                        {
                            Name = "Apples",
                            Slug = "apples",
                            Description = "Juicy apples",
                            Price = 1.50m,
                            Category = fruits,
                            Image = "apples.jpg",
                            VendorId = vendor1.Id, 
                            IsApproved = true,
                            IsAvailable = true,
                            StockQuantity = 100
                        },
                        new Product
                        {
                            Name = "Bananas",
                            Slug = "bananas",
                            Description = "Fresh bananas",
                            Price = 3m,
                            Category = fruits,
                            Image = "bananas.jpg",
                            VendorId = vendor2.Id,
                            IsApproved = true,
                            IsAvailable = true,
                            StockQuantity = 50
                        },
                        new Product
                        {
                            Name = "Watermelon",
                            Slug = "watermelon",
                            Description = "Juicy watermelon",
                            Price = 0.50m,
                            Category = fruits,
                            Image = "watermelon.jpg",
                            VendorId = vendor2.Id,
                            IsApproved = true,
                            IsAvailable = true,
                            StockQuantity = 5
                        },
                    new Product
                    {
                        Name = "Grapefruit",
                        Slug = "grapefruit",
                        Description = "Juicy grapefruit",
                        Price = 2m,
                        Category = fruits,
                        Image = "grapefruit.jpg",
                        VendorId = vendor1.Id,
                        IsApproved = true,
                        IsAvailable = true,
                        StockQuantity = 7
                    },
                    new Product
                    {
                        Name = "White shirt",
                        Slug = "white-shirt",
                        Description = "White shirt",
                        Price = 5.99m,
                        Category = shirts,
                        Image = "white shirt.jpg",
                        VendorId = vendor2.Id,
                        IsApproved = true,
                        IsAvailable = true,
                        StockQuantity = 20
                    },
                    new Product
                    {
                        Name = "Black shirt",
                        Slug = "black-shirt",
                        Description = "Black shirt",
                        Price = 7.99m,
                        Category = shirts,
                        Image = "black shirt.jpg",
                        VendorId = vendor1.Id,
                        IsApproved = true,
                        IsAvailable = true,
                        StockQuantity = 30
                    },
                    new Product
                    {
                        Name = "Yellow shirt",
                        Slug = "yellow-shirt",
                        Description = "Yellow shirt",
                        Price = 5.99m,
                        Category = shirts,
                        Image = "yellow shirt.jpg",
                        VendorId = vendor1.Id,
                        IsApproved = true,
                        IsAvailable = true,
                        StockQuantity = 20
                    }
            
                );

                context.SaveChanges();
            }
        }
    }
}
