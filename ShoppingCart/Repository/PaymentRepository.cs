using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Repository.BankSystem;
using ShoppingCart.Repository.BankSystem.BankSystemModels;
using static Grpc.Core.Metadata;

namespace ShoppingCart.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private IBank _bank;
        private readonly DataContext _context;
        public const decimal VendorPercentage = (decimal)0.8; 
        public const decimal SystemPercentage = (decimal)0.2;
        public PaymentRepository(IBank bank, DataContext context)
        {
            this._bank = bank;
            this._context = context;
        }



        public Status OrderPayment(OrderPaymentData orderPaymentData)
        {


            // Amount verification
            PaymentData data = new PaymentData();
            data.userAccount = orderPaymentData.Account;
            var result = _bank.VerifyPayment(data);
            if (result.Result.StatusCode == 0)
            {
                return result.Result;
            }


            // split order items to specific vendors
            IDictionary<string, decimal> Vendors = new Dictionary<string, decimal>();

            foreach (var item in orderPaymentData.Order.OrderItems)
            {
                string VendorId = item.Product.VendorId;

                if (Vendors.ContainsKey(VendorId))
                {
                    Vendors[VendorId] = Vendors[VendorId] + (item.Product.Price * item.Quantity);
                }
                else
                {
                    Vendors.Add(VendorId, item.Product.Price * item.Quantity);
                }
            }


            //80% payment of order for each vendor
            foreach(var entry in Vendors)
            {
                UserAccount VendorAccount = _context.UsersAccounts.Where(a=>a.UserId.Equals(entry.Key)).FirstOrDefault();
                
                PaymentData VendorPaymentData = new PaymentData();
                VendorPaymentData.userAccount = VendorAccount;


                VendorPaymentData.Amount = entry.Value * VendorPercentage;
                _bank.ProceedPayment(VendorPaymentData);
            
            }
           

            // 20% payment to the system

            PaymentData SystemPaymentData = new PaymentData();
            UserAccount SystemAccount = _context.UsersAccounts.Where(a => a.UserId.Equals("ShoppingCart")).FirstOrDefault();
            SystemPaymentData.Amount = orderPaymentData.Order.TotalAmount * SystemPercentage;
            SystemPaymentData.userAccount = SystemAccount;
            result =  _bank.ProceedPayment(SystemPaymentData);
            return result.Result;
        }


        //Vendor payment

            public Status VendorPayment(VendorPaymentData paymentData)
             {
            PaymentData data = new PaymentData();
            data.Amount = 20000;//To do: should not be hardcoded
            data.userAccount = paymentData.VendorAccount;
            var result  = _bank.VerifyPayment(data);
            if(result.Result.StatusCode==0)
            {
                return result.Result;
            }    
            result =  _bank.ProceedPayment(data);
            return result.Result;
           }
    }
}
