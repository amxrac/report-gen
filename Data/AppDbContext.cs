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
    }
}
