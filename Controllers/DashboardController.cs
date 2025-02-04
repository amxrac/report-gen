using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                if (roles.Contains("Veterinary Doctor"))
                    return RedirectToAction("VeterinaryForm");
                if (roles.Contains("Specialist"))
                    return RedirectToAction("SpecialistForm");
                if (roles.Contains("Health Officer"))
                    return RedirectToAction("HealthOfficerForm");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> SubmittedForms()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();

            List<object> forms = userRole switch
            {
                "Environmentalist" => (await _context.EnvironmentalistForms
                    .Where(f => f.SubmittedByUserId == user.Id)
                    .OrderByDescending(f => f.DateSubmitted)
                    .ToListAsync())
                    .Cast<object>()
                    .ToList(),

                "Veterinary Doctor" => (await _context.VeterinaryForms
                    .Where(f => f.SubmittedByUserId == user.Id)
                    .OrderByDescending(f => f.DateSubmitted)
                    .ToListAsync())
                    .Cast<object>()
                    .ToList(),

                "Specialist" => (await _context.SpecialistForms
                    .Where(f => f.SubmittedByUserId == user.Id)
                    .OrderByDescending(f => f.DateSubmitted)
                    .ToListAsync())
                    .Cast<object>()
                    .ToList(),

                "Health Officer" => (await _context.HealthOfficerForms
                    .Where(f => f.SubmittedByUserId == user.Id)
                    .OrderByDescending(f => f.DateSubmitted)
                    .ToListAsync())
                    .Cast<object>()
                    .ToList(),

                _ => new List<object>()
            };

            ViewBag.FormType = userRole?.Replace(" ", "") + "Form";

            return View("SubmittedForms", forms);
        }

        [HttpGet]
        public async Task<IActionResult> EnvironmentalistForm(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id.HasValue)
            {
                var existingForm = await _context.EnvironmentalistForms
                    .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id);

                if (existingForm == null) return NotFound();
                var model = new EnvironmentalistFormVM
                {
                    Id = existingForm.Id,
                    DateSubmitted = existingForm.DateSubmitted,
                    Location = existingForm.Location,
                    Description = existingForm.Description,
                    WitnessDetails = existingForm.WitnessDetails,
                    Category = existingForm.Category,
                    ActionsTaken = existingForm.ActionsTaken,
                    IsReported = existingForm.IsReported,
                    ReportedDetails = existingForm.ReportedDetails,
                    FollowUpAction = existingForm.FollowUpAction,
                };
                return View(model);

            }

            return View(new EnvironmentalistFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> EnvironmentalistForm(EnvironmentalistFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var isNewForm = model.Id == 0;
                EnvironmentalistForm formData = isNewForm
            ? new EnvironmentalistForm()
            : await _context.EnvironmentalistForms
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.SubmittedByUserId == user.Id);

                if (formData == null) return NotFound();

                formData.Id = model.Id;
                formData.DateSubmitted = model.DateSubmitted;
                formData.SubmittedByUser = user;
                formData.Location = model.Location;
                formData.Description = model.Description;
                formData.WitnessDetails = model.WitnessDetails;
                formData.Category = model.Category;
                formData.ActionsTaken = model.ActionsTaken;
                formData.IsReported = model.IsReported;
                formData.ReportedDetails = model.ReportedDetails;
                formData.FollowUpAction = model.FollowUpAction;

                if (isNewForm)
                {
                    _context.EnvironmentalistForms.Add(formData);
                }
                else
                {
                    _context.EnvironmentalistForms.Update(formData);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = isNewForm
                ? "Environmentalist Form submitted successfully!"
                : "Environmentalist Form updated successfully!";

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
        public async Task<IActionResult> VeterinaryForm(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id.HasValue)
            {
                var existingForm = await _context.VeterinaryForms
                    .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id);

                if (existingForm == null) return NotFound();
                var model = new VeterinaryFormVM
                {
                    Id = existingForm.Id,
                    DateSubmitted = existingForm.DateSubmitted,
                    AnimalSpecies = existingForm.AnimalSpecies  ,
                    HealthStatus = existingForm.HealthStatus,
                    VaccinationDetails = existingForm.VaccinationDetails,
                    ClinicalSymptoms = existingForm.ClinicalSymptoms,
                    PreliminaryDiagnosis = existingForm.PreliminaryDiagnosis,
                    PotentialZoonoticRisk = existingForm.PotentialZoonoticRisk,
                    SuspectedDisease = existingForm.SuspectedDisease,
                    QuarantineRecommended = existingForm.QuarantineRecommended,
                    FollowUpProtocol = existingForm.FollowUpProtocol,
                };
                return View(model);

            }

            return View(new VeterinaryFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> VeterinaryForm(VeterinaryFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var isNewForm = model.Id == 0;
                VeterinaryDoctorForm formData = isNewForm
            ? new VeterinaryDoctorForm()
            : await _context.VeterinaryForms
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.SubmittedByUserId == user.Id);

                if (formData == null) return NotFound();

                formData.Id = model.Id;
                formData.DateSubmitted = model.DateSubmitted;
                formData.SubmittedByUser = user;
                formData.AnimalSpecies = model.AnimalSpecies;
                formData.HealthStatus = model.HealthStatus;
                formData.VaccinationDetails = model.VaccinationDetails;
                formData.ClinicalSymptoms = model.ClinicalSymptoms;
                formData.PreliminaryDiagnosis = model.PreliminaryDiagnosis;
                formData.PotentialZoonoticRisk = model.PotentialZoonoticRisk;
                formData.SuspectedDisease = model.SuspectedDisease;
                formData.QuarantineRecommended = model.QuarantineRecommended;
                formData.FollowUpProtocol = model.FollowUpProtocol;

                if (isNewForm)
                {
                    _context.VeterinaryForms.Add(formData);
                }
                else
                {
                    _context.VeterinaryForms.Update(formData);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = isNewForm
                ? "Veterinary Form submitted successfully!"
                : "Veterinary Form updated successfully!";

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
        public async Task<IActionResult> SpecialistForm(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id.HasValue)
            {
                var existingForm = await _context.SpecialistForms
                    .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id);

                if (existingForm == null) return NotFound();
                var model = new SpecialistFormVM
                {
                    Id = existingForm.Id,
                    DateSubmitted = existingForm.DateSubmitted,
                    CaseType = existingForm.CaseType,
                    SeverityLevel = existingForm.SeverityLevel,
                    AffectedDemographic = existingForm.AffectedDemographic?.Split(',').ToList(),
                    TransmissionPattern = existingForm.TransmissionPattern,
                    ExposureHistory = existingForm.ExposureHistory,
                    LaboratoryFindings = existingForm.LaboratoryFindings,
                    AntibioticResistanceObserved = existingForm.AntibioticResistanceObserved,
                    RequiresNCDCNotification = existingForm.RequiresNCDCNotification,
                    ContainmentMeasures = existingForm.ContainmentMeasures,
                    SpecialistComments = existingForm.SpecialistComments
                };
                return View(model);

            }

            return View(new SpecialistFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> SpecialistForm(SpecialistFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var isNewForm = model.Id == 0;
                SpecialistForm formData = isNewForm
            ? new SpecialistForm()
            : await _context.SpecialistForms
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.SubmittedByUserId == user.Id);

                if (formData == null) return NotFound();

                formData.Id = model.Id;
                formData.DateSubmitted = model.DateSubmitted;
                formData.SubmittedByUser = user;
                formData.CaseType = model.CaseType;
                formData.SeverityLevel = model.SeverityLevel;
                formData.AffectedDemographic = string.Join(",", model.AffectedDemographic);
                formData.TransmissionPattern = model.TransmissionPattern;
                formData.ExposureHistory = model.ExposureHistory;
                formData.LaboratoryFindings = model.LaboratoryFindings;
                formData.AntibioticResistanceObserved = model.AntibioticResistanceObserved;
                formData.RequiresNCDCNotification = model.RequiresNCDCNotification;
                formData.ContainmentMeasures = model.ContainmentMeasures;
                formData.SpecialistComments = model.SpecialistComments;

                if (isNewForm)
                {
                    _context.SpecialistForms.Add(formData);
                }
                else
                {
                    _context.SpecialistForms.Update(formData);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = isNewForm
                ? "Public Health Specialist Form submitted successfully!"
                : "Public Health Specialist Form updated successfully!";

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
        public async Task<IActionResult> HealthOfficerForm(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id.HasValue)
            {
                var existingForm = await _context.HealthOfficerForms
                    .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id);

                if (existingForm == null) return NotFound();
                var model = new HealthOfficerFormVM
                {
                    Id = existingForm.Id,
                    DateSubmitted = existingForm.DateSubmitted,
                    FacilityType = existingForm.FacilityType,
                    InspectionResults = existingForm.InspectionResults,
                    SanitationStatus = existingForm.SanitationStatus,
                    PublicHealthRisk = existingForm.PublicHealthRisk,
                    DiseaseVectorPresent = string.Join(",", existingForm.DiseaseVectorPresent),
                    WaterQualityAssessment = existingForm.WaterQualityAssessment,
                    WasteDisposalEvaluation = existingForm.WasteDisposalEvaluation,
                    ComplianceStatus = existingForm.ComplianceStatus,
                    EnforcementMeasures = existingForm.EnforcementMeasures,
                    PublicHealthGuidance = existingForm.PublicHealthGuidance
                };
                return View(model);

            }

            return View(new HealthOfficerFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> HealthOfficerForm(HealthOfficerFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var isNewForm = model.Id == 0;
                HealthOfficerForm formData = isNewForm
            ? new HealthOfficerForm()
            : await _context.HealthOfficerForms
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.SubmittedByUserId == user.Id);

                if (formData == null) return NotFound();

                formData.Id = model.Id;
                formData.DateSubmitted = model.DateSubmitted;
                formData.SubmittedByUser = user;
                formData.FacilityType = model.FacilityType;
                formData.InspectionResults = model.InspectionResults;
                formData.SanitationStatus = model.SanitationStatus;
                formData.PublicHealthRisk = model.PublicHealthRisk;
                formData.DiseaseVectorPresent = string.Join(",", model.DiseaseVectorPresent);
                formData.WaterQualityAssessment = model.WaterQualityAssessment;
                formData.WasteDisposalEvaluation = model.WasteDisposalEvaluation;
                formData.ComplianceStatus = model.ComplianceStatus;
                formData.EnforcementMeasures = model.EnforcementMeasures;
                formData.PublicHealthGuidance = model.PublicHealthGuidance;

                if (isNewForm)
                {
                    _context.HealthOfficerForms.Add(formData);
                }
                else
                {
                    _context.HealthOfficerForms.Update(formData);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = isNewForm
                ? "Health Officer Form submitted successfully!"
                : "Health Officer Form updated successfully!";

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
