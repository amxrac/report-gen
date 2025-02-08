using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using rgproj.Models;
using rgproj.ViewModels;

namespace rgproj.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<EnvironmentalistForm> EnvironmentalistForms { get; set; }
        public DbSet<VeterinaryDoctorForm> VeterinaryForms { get; set; }
        public DbSet<SpecialistForm> SpecialistForms { get; set; }
        public DbSet<HealthOfficerForm> HealthOfficerForms { get; set; }
        public DbSet<GeneratedReport> GeneratedReports { get; set; }
        public async Task<List<object>> GetFormsByIds(List<int> selectedFormIds)
        {
            var forms = new List<object>();

            var environmentalistForms = await EnvironmentalistForms
                .Where(f => selectedFormIds.Contains(f.Id))
                .ToListAsync();
            forms.AddRange(environmentalistForms);

            var veterinaryForms = await VeterinaryForms
                .Where(f => selectedFormIds.Contains(f.Id))
                .ToListAsync();
            forms.AddRange(veterinaryForms);

            var specialistForms = await SpecialistForms
                .Where(f => selectedFormIds.Contains(f.Id))
                .ToListAsync();
            forms.AddRange(specialistForms);

            var healthOfficerForms = await HealthOfficerForms
                .Where(f => selectedFormIds.Contains(f.Id))
                .ToListAsync();
            forms.AddRange(healthOfficerForms);

            return forms;
        }

    }
}
