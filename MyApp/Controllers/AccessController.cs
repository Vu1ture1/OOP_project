using Microsoft.AspNetCore.Mvc;
using MyApp.DataBaseFolder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
            var user_pass = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == username).FirstOrDefault();

            byte[] salt = Convert.FromBase64String(user_pass.salt);

            password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


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

            user.path_to_channel_icon = "https://localhost:7012/Avatar_png/blank-profile-picture-973460_1280.png";

            user.path_to_channel_image = "https://localhost:7012/Article_png/footer.png";

            byte[] salt = new byte[128 / 8];
            
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Хэширование пароля с использованием PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            string salt_str = Convert.ToBase64String((byte[])salt);

            user.password = hashed;

            user.salt = salt_str;


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
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            return View(user);
        }

        public IActionResult Icon()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            return View();
        }

        public IActionResult ChannelIcon() 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            return View();
        }

        public IActionResult ChannelHeader() 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            return View();
        }
        public IActionResult DefaultIcon(string option) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            if (option == "1")
            {
                user.path_to_icon = "https://localhost:7012/Avatar_png/blank-profile-picture-973460_1280.png";
            }
            else if (option == "2") 
            {
                user.path_to_channel_icon = "https://localhost:7012/Avatar_png/blank-profile-picture-973460_1280.png";
            }
            else if (option == "3")
            {
                user.path_to_channel_image = "https://localhost:7012/Article_png/footer.png";
            }


            context.SaveChanges();

            return RedirectToAction("Account", "Access");
        }
        public IActionResult ChangeUsername(string stat) 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ViewData["username_info"] = stat;

            return View();
        }

        public IActionResult ChangeUsernameProccess(string username, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            if (user.username == username)
            {
                return RedirectToAction("ChangeUsername", "Access", new { stat = "same" });
            }

            var user_entered = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == username).FirstOrDefault();

            if (user_entered != null && user_entered.username == username)
            {
                return RedirectToAction("ChangeUsername", "Access", new { stat = "have" });
            }


            byte[] salt = Convert.FromBase64String(user.salt);

            password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (user.password == password)
            {
                user.username = username;

                context.SaveChanges();
            }
            else 
            {
                return RedirectToAction("ChangeUsername", "Access", new { stat = "passw" });
            }

            return RedirectToAction("Logout", "Access");
        }

        public IActionResult ChangeEmail(string stat)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ViewData["username_info"] = stat;

            return View();
        }

        public IActionResult ChangeEmailProccess(string email, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            if (user.user_info.email == email)
            {
                return RedirectToAction("ChangeEmail", "Access", new { stat = "same" });
            }

            var user_entered = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.user_info.email == email).FirstOrDefault();

            if (user_entered != null && user_entered.user_info.email == email)
            {
                return RedirectToAction("ChangeEmail", "Access", new { stat = "have" });
            }


            byte[] salt = Convert.FromBase64String(user.salt);

            password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (user.password == password)
            {
                user.user_info.email = email;

                context.SaveChanges();
            }
            else
            {
                return RedirectToAction("ChangeUsername", "Access", new { stat = "passw" });
            }

            return RedirectToAction("Logout", "Access");
        }

        public IActionResult Channel() 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.categories).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            return View(user);
        }

        public IActionResult ViewChannel(int userId) 
        {
            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.categories).Include(sub => sub.my_subscribes).Where<User>(var => var.id == userId).FirstOrDefault();

            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                ClaimsPrincipal ClUser = HttpContext.User;

                List<Claim> cl = ClUser.Claims.ToList();

                var user_viewer = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

                ViewData["CheckSubStr"] = "not sub";

                ViewData["CheckLikeStr"] = "not like";

                foreach (User us in user_viewer.my_subscribes)
                {
                    if (us.id == user.id)
                    {
                        ViewData["CheckSubStr"] = "sub";
                    }
                }

            }
            else
            {
                ViewData["CheckSubStr"] = "not entered";
            }

            return View(user);
        }
        public IActionResult ChangeChannelname(string stat)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ViewData["username_info"] = stat;

            return View();
        }

        public IActionResult ChangeChannelnameProccess(string channelname, string password)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            if (user.channelname == channelname)
            {
                return RedirectToAction("ChangeChannelname", "Access", new { stat = "same" });
            }

            var user_entered = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).Include(sub => sub.my_subscribes).Where<User>(var => var.channelname == channelname).FirstOrDefault();

            if (user_entered != null && user_entered.channelname == channelname)
            {
                return RedirectToAction("ChangeChannelname", "Access", new { stat = "have" });
            }


            byte[] salt = Convert.FromBase64String(user.salt);

            password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (user.password == password)
            {
                user.channelname = channelname;

                context.SaveChanges();
            }
            else
            {
                return RedirectToAction("ChangeChannelname", "Access", new { stat = "passw" });
            }

            return RedirectToAction("Channel", "Access");
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile fileUpload)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

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

        [HttpPost]
        public async Task<IActionResult> ChannelUpload(IFormFile fileUpload, string option)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            var filepath = "";

            string servermapath;// = Path.Combine(_env.WebRootPath, "Avatar_png", fileUpload.FileName);

            if (option == "1") 
            {
                servermapath = Path.Combine(_env.WebRootPath, "Avatar_png", fileUpload.FileName);
            }
            else 
            {
                servermapath = Path.Combine(_env.WebRootPath, "Article_png", fileUpload.FileName);
            }
            //C:\repos\MyApp\MyApp\wwwroot\Image

            using (var stream = new FileStream(servermapath, FileMode.Create))
            {
                fileUpload.CopyTo(stream);
            }

            
            if (option == "1")
            {
                filepath = "https://localhost:7012/" + "Avatar_png/" + fileUpload.FileName;
            }
            else
            {
                filepath = "https://localhost:7012/" + "Article_png/" + fileUpload.FileName;
            }
            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            foreach (Claim claim in cl) { Console.WriteLine(claim.Value); };

            var user = context.Users.Include(u => u.user_info).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            
            if (option == "1")
            {
                user.path_to_channel_icon = filepath;
            }
            else
            {
                user.path_to_channel_image = filepath;
            }

            context.SaveChanges();

            return RedirectToAction("Account", "Access");
        }

        [HttpPost]
        public async Task<IActionResult> ChannelSubscribe(int userId) 
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

            return RedirectToAction("ViewChannel", "Access", new { userId = userId});
        }

        [HttpPost]
        public async Task<IActionResult> ChannelUnSubscribe(int userId) 
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

            return RedirectToAction("ViewChannel", "Access", new { userId = userId });
        }

        [HttpPost]
        public IActionResult ChangeDesChannel() 
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.categories).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            return View(user);
        }

        [HttpPost]

        public IActionResult ChangeDesChannelProccess(string description)
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Access");
            }

            ClaimsPrincipal ClUser = HttpContext.User;

            List<Claim> cl = ClUser.Claims.ToList();

            var user = context.Users.Include(u => u.user_info).Include(a => a.channel_articles).ThenInclude(u => u.comments).ThenInclude(u => u.creator).Include(a => a.channel_articles).ThenInclude(u => u.categories).Include(sub => sub.my_subscribes).Where<User>(var => var.username == cl[0].Value && var.user_info.email == cl[2].Value).FirstOrDefault();

            user.channel_description = description;

            context.SaveChanges();

            return RedirectToAction("Channel", "Access");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
