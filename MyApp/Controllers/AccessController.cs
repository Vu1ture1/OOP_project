using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace MyApp.Controllers
{
    public class AccessController : Controller
    {
        private DBContext context;
        public AccessController(DBContext con) { context = con; }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal ClUser = HttpContext.User;

            if (ClUser.Identity.IsAuthenticated) 
            {
                if (ClUser.IsInRole("user") == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (ClUser.IsInRole("modder") == true) 
                {
                    return RedirectToAction("Index", "Modder");
                }
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (context.Users.Any(var => var.username == username && var.password == password) == true) 
            {
                var user = context.Users.Where<User>(var => var.username == username && var.password == password).FirstOrDefault();

                var claims = new List<Claim>() { new Claim(ClaimTypes.Name, username),
                                                 new Claim(ClaimTypes.Role, user.user_role),
                                                 new Claim(ClaimTypes.Email, user.user_info.email)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties prop = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(identity), prop);

                Console.WriteLine(user.user_role);

                return RedirectToAction("Index", "Home");
            }

            ViewData["NotFoundMessage"] = "User not found";
            
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (context.Users.Any(var => var.user_info.email == user.user_info.email) == true)
            {
                //тут сообщение об ошибке тк email уже есть
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                user.user_role = "user";

                context.Users.Add(user);

                context.SaveChanges();
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
