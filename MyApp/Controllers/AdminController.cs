using Microsoft.AspNetCore.Http;
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
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public AdminController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }

        
        public IActionResult Index(int skip)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            if (HttpContext.User.IsInRole("admin") == false)
            {
                return RedirectToAction("Login", "Access");
            }


            IEnumerable<User> Users;

            Users = context.Users.Include(u => u.user_info).Where(u => u.user_role != "admin").Skip((skip * 10)).Take(10).ToList();

            if (Users.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Users);
        }

        public IActionResult Next(int skip)
        {
            skip++;

            return RedirectToAction("Index", "Admin", new { skip = skip });
        }

        public IActionResult Back(int skip)
        {
            if (skip == 0)
            {
                return RedirectToAction("Index", "Admin", new { skip = skip });
            }

            skip--;

            return RedirectToAction("Index", "Home", new { skip = skip });
        }

        public IActionResult DefaultUser(int userId) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user.user_role = "user";

            context.SaveChanges();

            return RedirectToAction("Index", "Admin", new { skip = 0 });
        }

        public IActionResult DeleteUser(int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.categories).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            foreach(Article art in user.channel_articles) 
            {
                context.Articles.Remove(art);
            }

            context.Users.Remove(user);

            context.SaveChanges();

            return RedirectToAction("Index", "Admin", new { skip = 0 });
        }

        public IActionResult UserModder(int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user.user_role = "modder";

            context.SaveChanges();

            return RedirectToAction("Index", "Admin", new { skip = 0 });
        }
    }
}
