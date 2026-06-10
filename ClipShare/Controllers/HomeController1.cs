using Microsoft.AspNetCore.Mvc;

namespace ClipShare.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
