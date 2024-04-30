using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace MyApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ArticleController(IWebHostEnvironment env) { _env = env; }

        [Authorize]
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult cringe(string input)
        {
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

            return View("cringe", input);
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
