using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using rgproj.Data;
using rgproj.Models;
using rgproj.ViewModels;
using System.Data;
using System.Linq;

namespace rgproj.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public DashboardController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.UserRole = roles.Any() ? roles[0] : "No Role Assigned";
            ViewBag.UserName = user.Name ?? user.UserName;
            if (roles.Contains("Environmentalist"))
            {
                ViewBag.RecentForms = _context.EnvironmentalistForms
                    .Where(f => f.SubmittedByUserId == user.Id)
                    .OrderByDescending(f => f.DateGenerated)
                    .Take(3)
                    .ToList();
            }
            //else if (roles.Contains
        
            return View();
        }
    
        public async Task<IActionResult> SubmitForm()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Environmentalist"))
                    return RedirectToAction("EnvironmentalistForm");
                if (roles.Contains("Health Worker"))
                    return RedirectToAction("HealthWorkerForm");
                if (roles.Contains("Veterinary Doctor"))
                    return RedirectToAction("VeterinaryDoctorForm");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> EnvironmentalistForm()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new EnvironmentalistVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnvironmentalistForm(EnvironmentalistVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                EnvironmentalistForm report = new()
                {
                    DateGenerated = model.DateGenerated,
                    SubmittedByUser = user,
                    Location = model.Location,
                    Description = model.Description,
                    WitnessDetails = model.WitnessDetails,
                    Category = model.Category,
                    ActionsTaken = model.ActionsTaken,
                    IsReported = model.IsReported,
                    ReportedDetails = model.ReportedDetails,
                    FollowUpAction = model.FollowUpAction
                };

                _context.EnvironmentalistForms.Add(report);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Environmentalist Form submitted successfully!";

                return RedirectToAction("Index");
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

            }

            return View(model);
        }
    }
}
