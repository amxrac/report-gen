using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rgproj.Data;
using rgproj.Models;
using rgproj.Services;
using rgproj.ViewModels;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace rgproj.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IAiReportService _reportService;

        public AdminController(UserManager<AppUser> userManager, AppDbContext context, IAiReportService reportService)
        {
            _userManager = userManager;
            _context = context;
            _reportService = reportService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //        return RedirectToAction("Login", "Account");
        //    var roles = await _userManager.GetRolesAsync(user);

        //    ViewBag.UserRole = roles.Any() ? roles[0] : "No Role Assigned";
        //}

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.UserRole = roles.Any() ? roles[0] : "No Role Assigned";
            var model = new AdminDashboardVM()
            {
                EnvironmentalistForms = await _context.EnvironmentalistForms.ToListAsync(),
                VeterinaryForms = await _context.VeterinaryForms.ToListAsync(),
                SpecialistForms = await _context.SpecialistForms.ToListAsync(),
                HealthOfficerForms = await _context.HealthOfficerForms.ToListAsync(),
                SelectedFormIds = TempData["SelectedForms"] as List<int> ?? new List<int>()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewForm(string formType, int id)
        {
            dynamic form = formType switch
            {
                "Environmentalist" => await _context.EnvironmentalistForms
                    .Include(f => f.SubmittedByUser)
                    .FirstOrDefaultAsync(f => f.Id == id),
                "Veterinary" => await _context.VeterinaryForms
                    .Include(f => f.SubmittedByUser)
                    .FirstOrDefaultAsync(f => f.Id == id),
                "Specialist" => await _context.SpecialistForms
                    .Include(f => f.SubmittedByUser)
                    .FirstOrDefaultAsync(f => f.Id == id),
                "HealthOfficer" => await _context.HealthOfficerForms
                    .Include(f => f.SubmittedByUser)
                    .FirstOrDefaultAsync(f => f.Id == id),
                _ => null
            };

            if (form == null) return NotFound();

            ViewBag.FormType = formType;
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessSelectedForms(List<int> SelectedFormIds)
        {
            Console.WriteLine($"Processing {SelectedFormIds?.Count ?? 0} selected forms");
            if (SelectedFormIds != null)
            {
                Console.WriteLine($"Received Form IDs: {string.Join(", ", SelectedFormIds)}");
            }
            if (SelectedFormIds == null || !SelectedFormIds.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one form";
                return RedirectToAction("Dashboard");
            }

            Console.WriteLine($"Forms received: {string.Join(", ", SelectedFormIds)}");

            if (SelectedFormIds.Count > 10)
            {
                Console.WriteLine("Too many forms selected");
                TempData["ErrorMessage"] = "Maximum of 10 forms can be selected";
                return RedirectToAction("Dashboard");
            }

            //HttpContext.Session.SetString("SelectedForms", JsonSerializer.Serialize(SelectedFormIds));
            //TempData["SelectedForms"] = SelectedFormIds;
            //TempData.Keep("SelectedForms");
            var serializedForms = JsonSerializer.Serialize(SelectedFormIds);
            HttpContext.Session.SetString("SelectedFormIds", serializedForms);

            return RedirectToAction("GenerateReport");
        }

        [HttpGet]
        public IActionResult GenerateReport()
        {
            Console.WriteLine("Entering GenerateReport GET method");
            var serializedForms = HttpContext.Session.GetString("SelectedFormIds");
            if (string.IsNullOrEmpty(serializedForms))
            {
                Console.WriteLine("No forms found in Session");
                TempData["ErrorMessage"] = "No forms selected. Please go back and select forms.";
                return RedirectToAction("Dashboard");
            }
            var selectedForms = JsonSerializer.Deserialize<List<int>>(serializedForms);

            Console.WriteLine($"Forms retrieved from Session: {string.Join(", ", selectedForms)}");

            if (selectedForms == null || !selectedForms.Any())
            {
                Console.WriteLine("Deserialized forms list is empty");
                TempData["ErrorMessage"] = "No forms selected. Please go back and select forms.";
                return RedirectToAction("Dashboard");
            }

            var model = new ReportGenerationVM
            {
                SelectedFormIds = selectedForms.ToList(),
                SelectedModel = "Gemini"
            };

            TempData["SelectedForms"] = selectedForms;
            TempData.Keep("SelectedForms");

            Console.WriteLine("Rendering GenerateReport view");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReport(ReportGenerationVM model)
        {
            if (model.SelectedFormIds == null || !model.SelectedFormIds.Any())
            {
                model.SelectedFormIds = TempData["SelectedForms"] as List<int> ?? new List<int>();
            }

            if (!model.SelectedFormIds.Any())
            {
                TempData["ErrorMessage"] = "No forms selected for report generation.";
                return RedirectToAction("Dashboard");
            }
            Console.WriteLine($"Forms in GenerateReport POST: {string.Join(", ", model.SelectedFormIds)}");

            Console.WriteLine($"Selected Form IDs: {string.Join(", ", model.SelectedFormIds)}");
            Console.WriteLine($"Forms from TempData: {string.Join(", ", model.SelectedFormIds ?? new List<int>())}");

            var forms = await _context.GetFormsByIds(model.SelectedFormIds);

            var reportContent = await _reportService.GenerateComprehensiveReportAsync(
                forms,
                model.SelectedModel,
                model.IsPublic
            );

            var report = new GeneratedReport
            {
                Content = reportContent,
                IsPublic = model.IsPublic,
                GeneratedDate = DateTime.Now,
                ModelUsed = model.SelectedModel
            };

            _context.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReportPreview", new { id = report.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ReportPreview(int id)
        {
            var report = await _context.GeneratedReports.FindAsync(id);
            if (report == null) return NotFound();

            return View(report);
        }


        [HttpPost("test-ollama")]
        public async Task<IActionResult> TestOllama()
        {
            var result = await _reportService.GenerateWithOllama("Say hello");
            result = Regex.Replace(result, "<.*?>", "").Trim();
            return Ok(new { result });
        }
    }

}   