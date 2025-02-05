using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rgproj.Data;
using rgproj.Models;
using rgproj.ViewModels;

namespace rgproj.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}


public class AdminController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public AdminController(UserManager<AppUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;

    }


    public async Task<IActionResult> Dashboard()
    {
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
    public IActionResult ProcessSelectedForms(List<int> selectedForms)
    {
        TempData["SelectedForms"] = selectedForms;

        if (selectedForms.Count > 10)
        {
            TempData["ErrorMessage"] = "Maximum of 10 forms can be selected";
            return RedirectToAction("Dashboard");
        }

        return RedirectToAction("GenerateReport");
    }
}

