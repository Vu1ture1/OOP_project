using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using System.Diagnostics;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _env;

        private DBContext context;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, DBContext con)
        {
            _logger = logger;
            _env = env;
            context = con;
        }

        public IActionResult Index()
        {
            IEnumerable<Article> Articles = context.Articles.Include(com => com.comments).Include(cat => cat.categories).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true).Take(30).ToList();

            return View(Articles);
        }

        public IActionResult CreateArticle() 
        {
            return RedirectToAction("Index", "ArticleController");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
