using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels;


using IronPdf;


namespace ShoppingCart.Controllers
{

    public class ReportController : Controller{
    public IActionResult Index()
    {
            // List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            // CartViewModel cartVM = new(cart);
        
            // ReportViewModel reportVm = new(cartVM);
             
            //ViewData["data"] = reportVm;

            return View();
    }

      
        public async Task<IActionResult> GenerateReport()
        {
         //Load HTML content from URL or file
            var htmlToPdf = new HtmlToPdf();

            ChromePdfRenderer renderer = new ChromePdfRenderer();

            renderer.RenderingOptions = new ChromePdfRenderOptions()
            {
                EnableJavaScript = true,
                RenderDelay = 500,
             
            };
            
            var html = await renderer.RenderUrlAsPdfAsync("http://localhost:3000/report");


            // Convert HTML to PDF bytes and return as a file
            var pdfBytes = html.Stream.ToArray();
            return File(pdfBytes, "application/pdf", "report.pdf");

        }

    }
    
}