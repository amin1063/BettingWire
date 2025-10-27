using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleNewsPortal.Data;
using SimpleNewsPortal.Models;

namespace SimpleNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var newsList = _context.News.OrderByDescending(n => n.CreatedDate).ToList();
            return View(newsList);
        }
    }

}
