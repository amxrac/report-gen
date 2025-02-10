using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rgproj.Data;
using rgproj.Models;

namespace rgproj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var approvedReports = await _context.GeneratedReports
               .Where(r => r.IsPublic)
               .OrderByDescending(r => r.GeneratedDate) 
               .ToListAsync();

            return View(approvedReports);
        }

        public async Task<IActionResult> Details(int id)
        {
            var report = await _context.GeneratedReports
                .FirstOrDefaultAsync(r => r.Id == id && r.IsPublic);

            if (report == null)
            {
                return NotFound("Report not found or not public.");
            }

            return View(report);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
