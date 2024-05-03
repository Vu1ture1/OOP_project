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
    public class ArticleController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;
        public ArticleController(IWebHostEnvironment env, DBContext con) { _env = env; context = con; }

        [Authorize]
        
        [HttpGet]
        public IActionResult Index()
        {
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
            var filepath = "";

            string servermapath = Path.Combine(_env.WebRootPath, "Article_png", fileUpload.FileName);
            //C:\repos\MyApp\MyApp\wwwroot\Image

            using (var stream = new FileStream(servermapath, FileMode.Create))
            {
                fileUpload.CopyTo(stream);
            }

            filepath = "https://localhost:7012/" + "Article_png/" + fileUpload.FileName;

            string pattern_cat = "[A-Za-z0-9А-Яа-я]+";

            List<Category> cats = new List<Category>();
            
            foreach (Match m in Regex.Matches(categories, pattern_cat))
            {
                Category cat = new Category();

                cat.categoty_str = m.Value;
                
                cats.Add(cat);
            }

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
            article.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            article.categories = cats;
            article.path_to_corer = filepath;

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            foreach (Claim claim in cl) { Console.WriteLine(claim.Value); };

            var user = context.Users.Include(u => u.user_info).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            article.user_id = user.id;

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
    }
}
