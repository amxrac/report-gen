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

        //[HttpGet]
        //public async Task<IActionResult> EditForm(int id)
        //{
        //    // 1. Get current user
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null) return RedirectToAction("Login", "Account");

        //    // 2. Get user's role
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var userRole = roles.FirstOrDefault();

        //    // 3. Find form only if user owns it
        //    dynamic form = userRole switch
        //    {
        //        "Environmentalist" => await _context.EnvironmentalistForms
        //            .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id),
        //        "Veterinary Doctor" => await _context.VeterinaryForms
        //            .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id),
        //        "Specialist" => await _context.SpecialistForms
        //            .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id),
        //        "Health Officer" => await _context.HealthOfficerForms
        //            .FirstOrDefaultAsync(f => f.Id == id && f.SubmittedByUserId == user.Id),
        //        _ => null
        //    };

        //    if (form == null) return NotFound();

        //    // 4. Map to ViewModel based on role
        //    dynamic vm = userRole switch
        //    {
        //        "Environmentalist" => new EnvironmentalistFormVM
        //        {
        //            Id = form.Id,
        //            DateSubmitted = form.DateSubmitted,
        //            SubmittedByUser = user,
        //            Location = form.Location,
        //            Description = form.Description,
        //            WitnessDetails = form.WitnessDetails,
        //            Category = form.Category,
        //            ActionsTaken = form.ActionsTaken,
        //            IsReported = form.IsReported,
        //            ReportedDetails = form.ReportedDetails,
        //            FollowUpAction = form.FollowUpAction
        //        },
        //        "Veterinary Doctor" => new VeterinaryFormVM
        //        {
        //            Id = form.Id,
        //            DateSubmitted = form.DateSubmitted,
        //            SubmittedByUser = user,
        //            AnimalSpecies = form.AnimalSpecies,
        //            HealthStatus = form.HealthStatus,
        //            VaccinationDetails = form.VaccinationDetails,
        //            ClinicalSymptoms = form.ClinicalSymptoms,
        //            PreliminaryDiagnosis = form.PreliminaryDiagnosis,
        //            PotentialZoonoticRisk = form.PotentialZoonoticRisk,
        //            SuspectedDisease = form.SuspectedDisease,
        //            QuarantineRecommended = form.QuarantineRecommended,
        //            FollowUpProtocol = form.FollowUpProtocol
        //        },
        //        "Specialist" => new SpecialistFormVM()
        //        {
        //            Id = form.Id,
        //            DateSubmitted = form.DateSubmitted,
        //            SubmittedByUser = user,
        //            CaseType = form.CaseType,
        //            SeverityLevel = form.SeverityLevel,
        //            AffectedDemographic = string.Join(",", form.AffectedDemographic),
        //            TransmissionPattern = form.TransmissionPattern,
        //            ExposureHistory = form.ExposureHistory,
        //            LaboratoryFindings = form.LaboratoryFindings,
        //            AntibioticResistanceObserved = form.AntibioticResistanceObserved,
        //            RequiresNCDCNotification = form.RequiresNCDCNotification,
        //            ContainmentMeasures = form.ContainmentMeasures,
        //            SpecialistComments = form.SpecialistComments
        //        },
        //        "Health Officer" => new HealthOfficerFormVM()
        //        {
        //            Id = form.Id,
        //            DateSubmitted = form.DateSubmitted,
        //            SubmittedByUser = user,
        //            FacilityType = form.FacilityType,
        //            InspectionResults = form.InspectionResults,
        //            SanitationStatus = form.SanitationStatus,
        //            PublicHealthRisk = form.PublicHealthRisk,
        //            DiseaseVectorPresent = string.Join(",", form.DiseaseVectorPresent),
        //            WaterQualityAssessment = form.WaterQualityAssessment,
        //            WasteDisposalEvaluation = form.WasteDisposalEvaluation,
        //            ComplianceStatus = form.ComplianceStatus,
        //            EnforcementMeasures = form.EnforcementMeasures,
        //            PublicHealthGuidance = form.PublicHealthGuidance
        //        },

        //        _ => null
        //    };

        //    return View(vm);
        //}



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
                VeterinaryDoctorForm formData = new()
                {
                    Id = model.Id,
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
                    Id = model.Id,
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

        [HttpGet]
        public async Task<IActionResult> HealthOfficerForm()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new HealthOfficerFormVM
            {
                SubmittedByUserId = user.Id,
                SubmittedByUser = user
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HealthOfficerForm(HealthOfficerFormVM model, string? returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                HealthOfficerForm formData = new()
                {
                    Id = model.Id,
                    DateSubmitted = model.DateSubmitted,
                    SubmittedByUser = user,
                    FacilityType = model.FacilityType,
                    InspectionResults = model.InspectionResults,
                    SanitationStatus = model.SanitationStatus,
                    PublicHealthRisk = model.PublicHealthRisk,
                    DiseaseVectorPresent = string.Join(",", model.DiseaseVectorPresent),
                    WaterQualityAssessment = model.WaterQualityAssessment,
                    WasteDisposalEvaluation = model.WasteDisposalEvaluation,
                    ComplianceStatus = model.ComplianceStatus,
                    EnforcementMeasures = model.EnforcementMeasures,
                    PublicHealthGuidance = model.PublicHealthGuidance
                };

                _context.HealthOfficerForms.Add(formData);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Health Officer Form submitted successfully!";

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
