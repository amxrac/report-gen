using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using rgproj.Models;

namespace rgproj.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<EnvironmentalistForm> EnvironmentalistForms { get; set; }
        public DbSet<VeterinaryForm> VeterinaryForms { get; set; }
    }
}
