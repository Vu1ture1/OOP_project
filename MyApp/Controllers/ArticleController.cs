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
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Security.Cryptography;

namespace MyApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public ArticleController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }

        [Authorize]
        
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            return View();
        }

        //[HttpPost]
        //public IActionResult cringe(string title, string input)
        //{
        //    //Console.WriteLine(content);
            
        //    List<int> f = new List<int>();
        //    List<int> s = new List<int>();
        //    List<string> url = new List<string>();

        //    //string input = "<figure class=\"media\"><oembed url=\"https://www.youtube.com/watch?v=Z2XADS1HGOE\"></oembed></figure>";
        //    //string videoId = input.Split("https://www.youtube.com/watch?v=")[1].Split("\"")[0];

        //    string pattern1 = "<figure class=\"media\">";
        //    string pattern2 = "</oembed></figure>";
        //    string pattern3 = "https:\\/\\/www\\.youtube\\.com\\/watch\\?v=[a-zA-Z0-9_-]+";

        //    MatchCollection matches_f = Regex.Matches(input, pattern1), matches_s = Regex.Matches(input, pattern2),
        //        matches_url = Regex.Matches(input, pattern3);

        //    foreach (Match match in matches_f)
        //    {
        //        f.Add(match.Index);
        //    }

        //    foreach (Match match in matches_s)
        //    {
        //        s.Add(match.Index + 17);
        //    }

        //    foreach (Match match in matches_url)
        //    {
        //        url.Add(match.Value);
        //    }

        //    for (int i = 0; i < f.Count; i++) 
        //    {
        //        input = input.Remove(f[i], s[i] - f[i] + 1);

        //        input = input.Insert(f[i], $"<iframe width=\"560\" height=\"315\" src=\"{url[i].Replace("watch?v=", "embed/")}\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>");
        //    }

        //    //string output = $"<iframe width=\"560\" height=\"315\" src=\"{embedUrl}\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";

        //    Article article = new Article();

        //    article.content = input;
        //    article.title = title;
        //    article.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        //    ClaimsPrincipal ClUser = HttpContext.User;

        //    List<Claim> cl = ClUser.Claims.ToList();

        //    foreach (Claim claim in cl) { Console.WriteLine(claim.Value); };

        //    var user = context.Users.Include(u => u.user_info).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();



        //    return View("cringe", input);
        //}

        [HttpPost]
        public IActionResult CreateRequest(string categories, string title, IFormFile fileUpload, string input)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var filepath = "";

            //C:\repos\MyApp\MyApp\wwwroot\Image


            if (fileUpload != null)
            {
                // Загружаем изображение в память
                using (var memoryStream = new MemoryStream())
                {
                    fileUpload.CopyTo(memoryStream);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using (var image = Image.Load(memoryStream))
                    {
                        // Проверяем ширину и высоту изображения
                        if (image.Width != 1920 || image.Height != 1080)
                        {
                            filepath = "https://localhost:7012/Article_png/base.png";
                            goto h1;
                        }
                    }

                    string servermapath = Path.Combine(_env.WebRootPath, "Article_png", fileUpload.FileName);

                    using (var stream = new FileStream(servermapath, FileMode.Create))
                    {
                        fileUpload.CopyTo(stream);
                    }

                    filepath = "https://localhost:7012/Article_png/" + fileUpload.FileName;
                }
            }
            else
            {
                filepath = "https://localhost:7012/Article_png/base.png";
            }

            h1:

            //if (fileUpload != null)
            //{
                
                
                
                
                
                
            //    string servermapath = Path.Combine(_env.WebRootPath, "Article_png", fileUpload.FileName);

            //    using (var stream = new FileStream(servermapath, FileMode.Create))
            //    {
            //        fileUpload.CopyTo(stream);
            //    }

            //    filepath = "https://localhost:7012/" + "Article_png/" + fileUpload.FileName;
            //}
            //else 
            //{
            //    filepath = "https://localhost:7012/" + "Article_png/" + "base.png";
            //}

            string pattern_cat = "[A-Za-z0-9А-Яа-я]+";

            Category cat = new Category();

            cat.categoty_str = categories;

            //Console.WriteLine(content);

            List<int> f = new List<int>();
            List<int> s = new List<int>();
            List<string> url = new List<string>();

            //string input = "<figure class=\"media\"><oembed url=\"https://www.youtube.com/watch?v=Z2XADS1HGOE\"></oembed></figure>";
            //string videoId = input.Split("https://www.youtube.com/watch?v=")[1].Split("\"")[0];

            string pattern1 = "<figure class=\"media\">";
            string pattern2 = "</oembed></figure>";
            string pattern3 = "https:\\/\\/www\\.youtube\\.com\\/watch\\?v=[a-zA-Z0-9_-]+";

            MatchCollection matches_f = Regex.Matches(input, pattern1), matches_s = Regex.Matches(input, pattern2),
                matches_url = Regex.Matches(input, pattern3);

            foreach (Match match in matches_f)
            {
                f.Add(match.Index);
            }

            foreach (Match match in matches_s)
            {
                s.Add(match.Index + 17);
            }

            foreach (Match match in matches_url)
            {
                url.Add(match.Value);
            }

            for (int i = 0; i < f.Count; i++)
            {
                input = input.Remove(f[i], s[i] - f[i] + 1);

                input = input.Insert(f[i], $"<iframe width=\"560\" height=\"315\" src=\"{url[i].Replace("watch?v=", "embed/")}\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>");
            }

            //string output = $"<iframe width=\"560\" height=\"315\" src=\"{embedUrl}\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";

            Article article = new Article();

            article.content = input;
            article.title = title;
            article.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            article.category = cat;
            article.path_to_corer = filepath;

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            article.user = user;

            //context.Articles.Add(article);

            //context.SaveChanges();

            ArticleRequest req = new ArticleRequest();

            req.article = article;

            context.Requests.Add(req);

            context.SaveChanges();



            var ArticleReques = context.Requests.FirstOrDefault();

            return RedirectToAction("Account", "Access");
        }

        [HttpPost]

        public ActionResult UploadImage(List<IFormFile> files) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var filepath = "";

            foreach (IFormFile photo in Request.Form.Files) 
            {
                string servermapath = Path.Combine(_env.WebRootPath, "Image", photo.FileName);
                //C:\repos\MyApp\MyApp\wwwroot\Image

                using (var stream = new FileStream(servermapath, FileMode.Create)) 
                {
                    photo.CopyTo(stream);
                }

                filepath = "https://localhost:7012/" + "Image/" + photo.FileName;

                //https://localhost:7012/
            }


            return Json(new { url = filepath });
        }

        public IActionResult ViewArticle(int articleId)
        {
            var Article = context.Articles.Include(com => com.comments).ThenInclude(c => c.creator).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where<Article>(var => var.id == articleId).FirstOrDefault();

            
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                ClaimsPrincipal ClUser = HttpContext.User;

                List<Claim> cl = ClUser.Claims.ToList();

                var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

                ViewData["CheckSubStr"] = "not sub";

                ViewData["CheckLikeStr"] = "not like";

                foreach (User us in user_viewer.my_subscribes)
                {
                    if (us.id == Article.user.id)
                    {
                        ViewData["CheckSubStr"] = "sub";
                    }
                }

                foreach (int id in user_viewer.Liked_articles) 
                {
                    if (Article.id == id) 
                    {
                        ViewData["CheckLikeStr"] = "like";
                    }
                }
            }
            else 
            {
                ViewData["CheckSubStr"] = "not entered";

                ViewData["CheckLikeStr"] = "not entered";
            }

            return View(Article);
        }

        public IActionResult DeleteArticle(int articleId) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.category).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var Article = context.Articles.Include(com => com.comments).ThenInclude(c => c.creator).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where<Article>(var => var.id == articleId).FirstOrDefault();

            user.channel_articles.Remove(Article);

            context.Articles.Remove(Article);

            context.SaveChanges();

            return RedirectToAction("Channel", "Access");
        }

        public IActionResult DeleteArticleRequest(int articleId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.category).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var Request = context.Requests.Include(u => u.article).Include(com => com.article.comments).Include(cat => cat.article.category).Include(t => t.article.user).Include(a => a.article.user.user_info).Where<ArticleRequest>(var => var.id == articleId).FirstOrDefault();

            Request.article.user.channel_articles.Remove(Request.article);

            Request.article.comments.Clear();

            context.Remove(Request.article.category);

            Request.article.category = null;

            context.Articles.Remove(Request.article);

            context.Remove(Request);

            context.SaveChanges();

            return RedirectToAction("Channel", "Access");
        }

        [HttpPost]
        public IActionResult SubscribeArticle(int articleId, int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }   

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var article_user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user_viewer.my_subscribes.Add(article_user);

            article_user.subscribers_num++;

            context.SaveChanges();

            return RedirectToAction("ViewArticle", "Article", new { articleId = articleId });
        }

        [HttpPost]
        public IActionResult UnSubscribeArticle(int articleId, int userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var article_user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            user_viewer.my_subscribes.Remove(article_user);

            article_user.subscribers_num--;

            context.SaveChanges();

            return RedirectToAction("ViewArticle", "Article", new { articleId = articleId });
        }

        [HttpPost]
        public IActionResult CommentCreation(string con, int articleId) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var Article = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where<Article>(var => var.id == articleId).FirstOrDefault();

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            Comment comment = new Comment();

            comment.context = con;

            comment.creator = user_viewer;

            comment.created = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Article.comments.Add(comment);

            context.SaveChanges();

            return RedirectToAction("ViewArticle", "Article", new { articleId = articleId });
        }

        [HttpPost]
        public IActionResult Like(int articleId)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var Article = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where<Article>(var => var.id == articleId).FirstOrDefault();

            user_viewer.Liked_articles.Add(Article.id);

            Article.likes++;

            context.SaveChanges();

            return RedirectToAction("ViewArticle", "Article", new { articleId = articleId });
        }

        [HttpPost]
        public IActionResult UnLike(int articleId)  
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            var Article = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where<Article>(var => var.id == articleId).FirstOrDefault();

            user_viewer.Liked_articles.Remove(Article.id);

            Article.likes--;

            context.SaveChanges();

            return RedirectToAction("ViewArticle", "Article", new { articleId = articleId });
        }
    }
}
