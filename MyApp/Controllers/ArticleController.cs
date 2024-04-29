using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult cringe(string content)
        {
            Console.WriteLine(content);

            return View("cringe", content);
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
