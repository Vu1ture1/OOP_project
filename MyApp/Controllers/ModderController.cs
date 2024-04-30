using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class ModderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
