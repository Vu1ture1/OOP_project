using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public AccessController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }
        
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
                var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == username && var.password == password).FirstOrDefault();

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
            user.path_to_icon = "https://localhost:7012/Avatar_png/blank-profile-picture-973460_1280.png";

            if (context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Any(var => var.user_info.email == user.user_info.email) == true)
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

            return RedirectToAction("Login", "Access");
        }

        public IActionResult Account() 
        {
            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            return View(user);
        }

        public IActionResult Icon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile fileUpload)
        {
            var filepath = "";

            string servermapath = Path.Combine(_env.WebRootPath, "Avatar_png", fileUpload.FileName);
            //C:\repos\MyApp\MyApp\wwwroot\Image

            using (var stream = new FileStream(servermapath, FileMode.Create))
            {
                fileUpload.CopyTo(stream);
            }

            filepath = "https://localhost:7012/" + "Avatar_png/" + fileUpload.FileName;

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            foreach (Claim claim in cl) { Console.WriteLine(claim.Value); };

            var user = context.Users.Include(u => u.user_info).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            user.path_to_icon = filepath;

            context.SaveChanges();

            return RedirectToAction("Account", "Access");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
