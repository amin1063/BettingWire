using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SimpleNewsPortal.Data;
using SimpleNewsPortal.Models;
using System.Linq;

namespace SimpleNewsPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Hardcoded credentials (you can later move them to config)
        private const string USERNAME = "admin";
        private const string PASSWORD = "123";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // LOGIN / LOGOUT
        // -------------------------
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, skip login page
            if (IsAdmin())
                return RedirectToAction("List");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == USERNAME && password == PASSWORD)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("List");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // -------------------------
        // ADMIN NEWS MANAGEMENT
        // -------------------------

        public IActionResult List()
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            var newsList = _context.News
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            return View(newsList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult Create(News model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                _context.News.Add(model);
                _context.SaveChanges();
                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            var news = _context.News.Find(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost]
        public IActionResult Edit(News model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.News.Update(model);
                _context.SaveChanges();
                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login");

            var news = _context.News.Find(id);
            if (news != null)
            {
                _context.News.Remove(news);
                _context.SaveChanges();
            }

            return RedirectToAction("List");
        }

        // -------------------------
        // SESSION CHECK
        // -------------------------
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }
    }
}
