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


//< option value = "Нет темы" > Нет темы </ option >
//< option value = "Спорт" > Спорт </ option >
//< option value = "Наука" > Наука </ option >
//< option value = "Еда" > Еда </ option >
//< option value = "Культура" > Культура </ option >
//< option value = "Экономика" > Экономика </ option >
//< option value = "Технологии" > Технологии </ option >
//< option value = "Техника" > Техника </ option >
//< option value = "Гейминг" > Гейминг </ option >
//< option value = "Путешествия" > Путешествия </ option >
//< option value = "Красота и стиль" > Красота и стиль</option>
//<option value = "Животные" > Животные </ option >


namespace MyApp.Controllers
{
    public class ThemeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private DBContext context;

        public ThemeController(IWebHostEnvironment env, DBContext con)
        {
            _env = env;
            context = con;
        }

        public IActionResult Next(int skip, int op)
        {
            skip++;

            if(op == 0)
            {
                return RedirectToAction("Index", "Theme", new { skip = skip });
            }
            else if(op == 1) 
            {
                return RedirectToAction("Sport", "Theme", new { skip = skip });
            }
            else if (op == 2)
            {
                return RedirectToAction("Science", "Theme", new { skip = skip });
            }
            else if (op == 3)
            {
                return RedirectToAction("Food", "Theme", new { skip = skip });
            }
            else if (op == 4)
            {
                return RedirectToAction("Culture", "Theme", new { skip = skip });
            }
            else if (op == 5)
            {
                return RedirectToAction("Economy", "Theme", new { skip = skip });
            }
            else if (op == 6)
            {
                return RedirectToAction("Tech", "Theme", new { skip = skip });
            }
            else if (op == 7)
            {
                return RedirectToAction("Mech", "Theme", new { skip = skip });
            }
            else if (op == 8)
            {
                return RedirectToAction("Gaming", "Theme", new { skip = skip });
            }
            else if (op == 9)
            {
                return RedirectToAction("Travel", "Theme", new { skip = skip });
            }
            else if (op == 10)
            {
                return RedirectToAction("BeautyAndStyle", "Theme", new { skip = skip });
            }
            else if (op == 11)
            {
                return RedirectToAction("Animals", "Theme", new { skip = skip });
            }

            return RedirectToAction("Index", "Theme", new { skip = skip });
        }

        public IActionResult Back(int skip, int op)
        {
            if (skip == 0)
            {
                if (op == 0)
                {
                    return RedirectToAction("Index", "Theme", new { skip = skip });
                }
                else if (op == 1)
                {
                    return RedirectToAction("Sport", "Theme", new { skip = skip });
                }
                else if (op == 2)
                {
                    return RedirectToAction("Science", "Theme", new { skip = skip });
                }
                else if (op == 3)
                {
                    return RedirectToAction("Food", "Theme", new { skip = skip });
                }
                else if (op == 4)
                {
                    return RedirectToAction("Culture", "Theme", new { skip = skip });
                }
                else if (op == 5)
                {
                    return RedirectToAction("Economy", "Theme", new { skip = skip });
                }
                else if (op == 6)
                {
                    return RedirectToAction("Tech", "Theme", new { skip = skip });
                }
                else if (op == 7)
                {
                    return RedirectToAction("Mech", "Theme", new { skip = skip });
                }
                else if (op == 8)
                {
                    return RedirectToAction("Gaming", "Theme", new { skip = skip });
                }
                else if (op == 9)
                {
                    return RedirectToAction("Travel", "Theme", new { skip = skip });
                }
                else if (op == 10)
                {
                    return RedirectToAction("BeautyAndStyle", "Theme", new { skip = skip });
                }
                else if (op == 11)
                {
                    return RedirectToAction("Animals", "Theme", new { skip = skip });
                }
            }

            skip--;

            if (op == 0)
            {
                return RedirectToAction("Index", "Theme", new { skip = skip });
            }
            else if (op == 1)
            {
                return RedirectToAction("Sport", "Theme", new { skip = skip });
            }
            else if (op == 2)
            {
                return RedirectToAction("Science", "Theme", new { skip = skip });
            }
            else if (op == 3)
            {
                return RedirectToAction("Food", "Theme", new { skip = skip });
            }
            else if (op == 4)
            {
                return RedirectToAction("Culture", "Theme", new { skip = skip });
            }
            else if (op == 5)
            {
                return RedirectToAction("Economy", "Theme", new { skip = skip });
            }
            else if (op == 6)
            {
                return RedirectToAction("Tech", "Theme", new { skip = skip });
            }
            else if (op == 7)
            {
                return RedirectToAction("Mech", "Theme", new { skip = skip });
            }
            else if (op == 8)
            {
                return RedirectToAction("Gaming", "Theme", new { skip = skip });
            }
            else if (op == 9)
            {
                return RedirectToAction("Travel", "Theme", new { skip = skip });
            }
            else if (op == 10)
            {
                return RedirectToAction("BeautyAndStyle", "Theme", new { skip = skip });
            }
            else if (op == 11)
            {
                return RedirectToAction("Animals", "Theme", new { skip = skip });
            }

            return RedirectToAction("Index", "Theme", new { skip = skip });
        }
        public IActionResult Index(int skip) // все
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true).OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Sport(int skip) // Спорт
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Спорт").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Science(int skip) // наука
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Наука").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }


        public IActionResult Food(int skip) // еда
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Еда").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Culture(int skip) // культура
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Культура").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Economy(int skip) // экономика
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Экономика").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Tech(int skip) // Технологии
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Технологии").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Mech(int skip) // Техника
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Техника").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Gaming(int skip) // Гейминг
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Гейминг").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Travel(int skip) // Путешествия
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Путешествия").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult BeautyAndStyle(int skip) // Красота и стиль
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Красота и стиль").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }

        public IActionResult Animals(int skip) // Животные
        {
            IEnumerable<Article> Articles;

            Articles = context.Articles.Include(com => com.comments).Include(cat => cat.category).Include(t => t.user).Include(a => a.user.user_info).Where(a => a.status == true && a.category.categoty_str == "Животные").OrderByDescending(e => e.id).Skip((skip * 10)).Take(10).ToList();

            if (Articles.Count() != 10)
            {
                skip = 0;
            }

            ViewData["skip"] = skip;

            return View(Articles);
        }
    }
}
