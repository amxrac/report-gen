using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
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
                    .OrderByDescending(f => f.DateSubmitted)
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
                    return RedirectToAction("VeterinaryForm");
                if (roles.Contains("Specialist"))
                    return RedirectToAction("SpecialistForm");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> EnvironmentalistForm()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new EnvironmentalistFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnvironmentalistForm(EnvironmentalistFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                EnvironmentalistForm formData = new()
                {
                    DateSubmitted = model.DateSubmitted,
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

                _context.EnvironmentalistForms.Add(formData);
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

        [HttpGet]
        public async Task<IActionResult> VeterinaryForm()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new VeterinaryFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VeterinaryForm(VeterinaryFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                VeterinaryForm formData = new()
                {
                    DateSubmitted = model.DateSubmitted,
                    SubmittedByUser = user,
                    AnimalSpecies = model.AnimalSpecies,
                    HealthStatus = model.HealthStatus,
                    VaccinationDetails = model.VaccinationDetails,
                    ClinicalSymptoms = model.ClinicalSymptoms,
                    PreliminaryDiagnosis = model.PreliminaryDiagnosis,
                    PotentialZoonoticRisk = model.PotentialZoonoticRisk,
                    SuspectedDisease = model.SuspectedDisease,
                    QuarantineRecommended = model.QuarantineRecommended,
                    FollowUpProtocol = model.FollowUpProtocol
                };

                _context.VeterinaryForms.Add(formData);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Veterinary Form submitted successfully!";

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

        [HttpGet]
        public async Task<IActionResult> SpecialistForm()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new SpecialistFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SpecialistForm(SpecialistFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                SpecialistForm formData = new()
                {
                    DateSubmitted = model.DateSubmitted,
                    SubmittedByUser = user,
                    CaseType = model.CaseType,
                    SeverityLevel = model.SeverityLevel,
                    AffectedDemographic = string.Join(",", model.AffectedDemographic),
                    TransmissionPattern = model.TransmissionPattern,
                    ExposureHistory = model.ExposureHistory,
                    LaboratoryFindings = model.LaboratoryFindings,
                    AntibioticResistanceObserved = model.AntibioticResistanceObserved,
                    RequiresNCDCNotification = model.RequiresNCDCNotification,
                    ContainmentMeasures = model.ContainmentMeasures,
                    SpecialistComments = model.SpecialistComments
                };

                _context.SpecialistForms.Add(formData);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Public Health Specialist Form submitted successfully!";

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
