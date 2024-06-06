using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using System.Diagnostics;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;

namespace MyApp.Controllers
{
    public class InformationController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public InformationController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }
        public ActionResult Likes()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.category).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            List<Article> liked = new List<Article>();

            for(int counter = user.Liked_articles.Count - 1; counter > -1; counter--) 
            {
                int articleId = user.Liked_articles[counter];

                liked.Add(context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.id == articleId).FirstOrDefault());
            }
            
            return View(liked);
        }

        public ActionResult Subs()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.category).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            return View(user.my_subscribes);
        }
    }
}
