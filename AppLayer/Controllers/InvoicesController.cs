using Microsoft.AspNetCore.Mvc;

namespace AppLayer.Controllers
{
    public class InvoicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
