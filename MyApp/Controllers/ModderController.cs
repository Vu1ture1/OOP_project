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

namespace MyApp.Controllers
{
    public class ModderController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public ModderController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false) 
            {
                return RedirectToAction("Login", "Access");
            }
            
            IEnumerable < ArticleRequest > ArticleReques = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.categories).Include(t => t.article.user).Include(a => a.article.user.user_info).Take(10).ToList();

            return View(ArticleReques);
        }

        public IActionResult ModdingArticle(int articleId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var art = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.categories).Include(t => t.article.user).Include(a => a.article.user.user_info).Where<ArticleRequest>(var => var.id == articleId).FirstOrDefault();

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            ViewData["CheckSub"] = false;

            foreach (User us in user_viewer.my_subscribes) 
            {
                if (us.id == art.article.user.id) 
                {
                    ViewData["CheckSub"] = true;
                }
            }

            return View(art);
        }

        [HttpPost]
        public IActionResult SubscribeArticle(int articleId, int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var art = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.categories).Include(t => t.article.user).Include(a => a.article.user.user_info).Where<ArticleRequest>(var => var.id == articleId).FirstOrDefault();

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var article_user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user_viewer.my_subscribes.Add(article_user);

            article_user.subscribers_num++;

            context.SaveChanges();

            return RedirectToAction("ModdingArticle", "Modder", new { articleId = articleId});
        }

        [HttpPost]
        public IActionResult UnSubscribeArticle(int articleId, int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var art = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.categories).Include(t => t.article.user).Include(a => a.article.user.user_info).Where<ArticleRequest>(var => var.id == articleId).FirstOrDefault();

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var article_user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user_viewer.my_subscribes.Remove(article_user);

            article_user.subscribers_num--;

            context.SaveChanges();

            return RedirectToAction("ModdingArticle", "Modder", new { articleId = articleId });
        }

        public IActionResult Decision(string dec, int articleId) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var art = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.categories).Include(t => t.article.user).Include(a => a.article.user.user_info).Where<ArticleRequest>(var => var.id == articleId).FirstOrDefault();

            bool ch = bool.Parse(dec);

            if (ch == false)
            {
                art.article.user.channel_articles.Remove(art.article);
                
                art.article.comments.Clear();

                art.article.categories.Clear();

                
                context.Articles.Remove(art.article);
                
                context.Remove(art);

                context.SaveChanges();
            }
            else 
            {
                context.Remove(art);

                art.article.status = true;

                context.SaveChanges();
            }

            return RedirectToAction("Index", "Modder");
        }
    }
}
