using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace MyApp.Controllers
{
    public class ModderController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public ModderController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }

        [HttpPost]
        public IActionResult Index()
        {
            var ArticleReques = context.Requests.FirstOrDefault();


            if (ArticleReques != null)
            {
                return View(ArticleReques);
            }
            else
            {
                
            }
            
            Console.Write("ksndviosd");

            return View(ArticleReques);
        }
    }
}
