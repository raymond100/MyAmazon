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


        public OrderController(DataContext context, UserManager<AppUser> userManager, IProductService productService
        ,IOrderService orderService, IOrderItemService orderItemService, IPaymentRepository paymentRepository)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _paymentRepository = paymentRepository;
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

            return View(new CartViewModel(new Cart { CartItems = cartItems }));
        }

         public async Task<IActionResult> PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);
            //var user = await _userManager.FindByIdAsync(userId);
            Guid orderNumber = Guid.NewGuid();

            DateTime currentDateTime = DateTime.Now;
            Cart userCart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);

            var orderItems = new List<OrderItem>();
            
            foreach (var cartItem in userCart.CartItems)
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

            Order data = new Order(userId,orderNumber.ToString(),currentDateTime,userCart.Total,orderItems);


            var userAccount = new UserAccount
            {
               Id = 30,
               UserId = "aaa",
               NameOnCard = "aaa",
               CardNumber = 0000000000000000,
               ExpirationDate = DateTime.Now,
               CVV = 000,
               PaymentType = PaymentType.VISA
            };

            OrderPaymentData orderPaymentData = new OrderPaymentData();
            orderPaymentData.Order = data;
            orderPaymentData.Account = userAccount;
            Status status = _paymentRepository.OrderPayment(orderPaymentData);
            if(status.StatusCode != 1)
            {
                TempData["msg"] = "order failed";
                return RedirectToAction("Index");
            }
          
            await _orderService.CreateOrderAsync(data);
            TempData["msg"] = "order Success";
            return RedirectToAction("Index");
        }
    }
}