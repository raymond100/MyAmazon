namespace ShoppingCart.Models.ViewModels
{
    public class ReportViewModel
    {
       public CartViewModel _cart { get; set; }

       public ReportViewModel(CartViewModel cart){
         _cart = cart;
       }
    }
}
