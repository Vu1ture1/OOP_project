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

        public IActionResult Index(int skip)
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true).OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;
            
            return View(Articles);
        }

        public IActionResult Next(int skip)
        {
            skip++;
            
            return RedirectToAction("Index", "Home", new { skip = skip });
        }

        public IActionResult Back(int skip)
        {
            if (skip == 0) 
            {
                return RedirectToAction("Index", "Home", new { skip = skip });
            }

            skip--;
            
            return RedirectToAction("Index", "Home", new { skip = skip });
        }

        public IActionResult CreateArticle() 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            return RedirectToAction("Index", "ArticleController");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
