using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Text;
using ShoppingCart.Repository;
using ShoppingCart.Repository.BankSystem;
using ShoppingCart.Repository.BankSystem.BankSystemModels;
using Stripe;
using Stripe.Checkout;


namespace ShoppingCart.Controllers
{
    public class OrderController: Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITaxRateRepository _rateRepository;
        private readonly ICartService _cartService;
        private readonly StripeSettings _stripeSettings;



        public OrderController(DataContext context, 
        UserManager<AppUser> userManager, 
        IProductService productService,
        IOrderService orderService, 
        IOrderItemService orderItemService, 
        IPaymentRepository paymentRepository,
        ITaxRateRepository rateRepository,
        ICartService cartService,
        StripeSettings stripeSettings
        )
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _paymentRepository = paymentRepository;
            _rateRepository = rateRepository;
            _cartService = cartService;
            _stripeSettings = stripeSettings;
        }

         public async Task<IActionResult> Index()
        {
            List<CartItem> cartItems;

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(User);
                Cart userCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                                        .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (userCart == null)
                {
                    return View(new CartViewModel(new Cart()));
                }

                cartItems = userCart.CartItems;
            }
            else
            {
                cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            }

            ShoppingCart.Models.TaxRate rate = await _rateRepository.GetLatestTaxRateAsync();

            return View(new CartViewModel(new Cart { CartItems = cartItems, Rate = rate }));
        }

         public async Task<IActionResult> PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);
            //var user = await _userManager.FindByIdAsync(userId);
            Guid orderNumber = Guid.NewGuid();
            DateTime currentDateTime = DateTime.Now;
            List<CartItem> cartItems;
            Cart userCart = null;

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(User);
                userCart = await _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Product)
                                     .Include(c => c.Rate)
                                     .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (userCart == null)
                {
                    return View();
                }

                cartItems = userCart.CartItems;
            }
            else
            {
                cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
                ShoppingCart.Models.TaxRate latestRate = await _rateRepository.GetLatestTaxRateAsync();
                userCart = new Cart
                {
                    UserId = null,
                    CartItems = cartItems,
                    Rate = latestRate
                   
                };
            }

            var orderItems = new List<OrderItem>();
            
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Product = cartItem.Product,
                    ProductName = cartItem.ProductName,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    OrderDate = currentDateTime
                   

                };
               // await _orderItemService.CreateOrderItemAsync(orderItem);
                orderItems.Add(orderItem);
            }
             decimal totalAmount = userCart.Total + (userCart.Total * userCart.Rate.Rate);

              // This is your test secret API key.
                StripeConfiguration.ApiKey = "sk_test_51N13lMJv4RTYGtGNGaPzVCygiZuP5DEk9JJK9xfllJmn3YXdJDRhVqM6pM8NPfy1oCbcnPVYbLRzGDc10dCrbpfw00mQj4hBVN";
                // Alternatively, set up a webhook to listen for the payment_intent.succeeded event
                // and attach the PaymentMethod to a new Customer
                var customers = new CustomerService();
                var customer = customers.Create(new CustomerCreateOptions());
                var paymentIntentService = new PaymentIntentService();

                try{
                    var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
                    {
                        Customer = customer.Id,
                        SetupFutureUsage = "off_session",
                        Amount = (long) (totalAmount * 100),
                        Currency = "usd",
                        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                        {
                          Enabled = true,
                        },
                        PaymentMethod = "pm_card_visa"
                    });

                    if(paymentIntent != null){                    
                        Order data = new Order(userId,orderNumber.ToString(),currentDateTime,totalAmount,orderItems,userCart.Rate);
                        await _orderService.CreateOrderAsync(data);
                        _cartService.ClearCartAsync();
                        TempData["SuccessMessage"] = "Thanks for your order!";
                    }else{
                          TempData["ErrorMessage"] = "Please add items to your cart!";
                    }
                    return Json(new { clientSecret = paymentIntent.ClientSecret });
                }catch(StripeException e){
                    TempData["ErrorMessage"] = "Payment processing error!";
                }

               
             TempData["ErrorMessage"] = "Please add items to your cart!";

            return RedirectToAction("Index");
        }
    }
}