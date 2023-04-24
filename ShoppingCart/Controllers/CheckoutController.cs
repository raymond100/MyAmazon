using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Repository;
using ShoppingCart.Repository.BankSystem;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Services;
using ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingCart.Controllers
{

    public class CheckoutController : Controller
    {

        private readonly IPaymentRepository paymentRepository;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;
        public readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public CheckoutController(IPaymentRepository paymentRepository, IUserRepository userRepository, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.paymentRepository = paymentRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
           
        }
        public IActionResult Index()
        {
            VendorPaymentData model = new VendorPaymentData();
            return View(model);

        }

        public async Task<IActionResult> VendorPayment(VendorPaymentData model)
        {
          var resunt =  paymentRepository.VendorPayment(model);
            if(resunt.StatusCode == 1)
            {
                AppUser user = await userManager.GetUserAsync(User);
                var role = await roleManager.FindByNameAsync("Vendor");
                if (role == null)
                {
                    // Create the "Customer" role if it doesn't exist
                    await roleManager.CreateAsync(new IdentityRole("Vendor"));

                    role = await roleManager.FindByNameAsync("Vendor");
                }

                await userManager.AddToRoleAsync(user, role.Name);
                UserAccount account = model.VendorAccount;
                account.UserId=user.Id;
                userRepository.SaveUserAccount(account);
            }
        

            TempData["msg"] = resunt.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrderPayment(OrderPaymentData model)
        {
    
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> VendorPayment(VendorPaymentData model)
        //{
        //  var resunt =  paymentRepository.VendorPayment(model);
        //    if(resunt.StatusCode == 1)
        //    {
        //        AppUser user = await userManager.GetUserAsync(User);
        //        var role = await roleManager.FindByNameAsync("Vendor");
        //        if (role == null)
        //        {
        //            // Create the "Customer" role if it doesn't exist
        //            await roleManager.CreateAsync(new IdentityRole("Vendor"));

        //            role = await roleManager.FindByNameAsync("Vendor");
        //        }

        //        await userManager.AddToRoleAsync(user, role.Name);
        //        UserAccount account = model.VendorAccount;
        //        account.UserId=user.Id;
        //        userRepository.SaveUserAccount(account);
        //    }
        

        //    TempData["msg"] = resunt.Message;
        //    return RedirectToAction(nameof(Index));
        //}

        
    }
}