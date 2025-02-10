using Microsoft.AspNetCore.Identity;
using rgproj.Models;

namespace rgproj.Data.Seeders
{
    public class AdminSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public AdminSeeder(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task SeedAdminAsync()
        {
            var adminEmail = "admin@admin.com";
            var adminPassword = "SecurePassword123!";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "System Admin",
                    Location = "HQ"
                };

                await _userManager.CreateAsync(adminUser, adminPassword);
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
