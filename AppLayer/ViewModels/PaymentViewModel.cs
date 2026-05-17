using Microsoft.AspNetCore.Mvc;

namespace AppLayer.ViewModels
{
    public class PaymentViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
