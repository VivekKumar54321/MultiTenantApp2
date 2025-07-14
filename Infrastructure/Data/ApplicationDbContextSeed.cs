using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure DB is created
            await context.Database.MigrateAsync();

            // Step 1: Seed default Tenant
            var defaultTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Identifier == "default-tenant");
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant
                {
                    Name = "Default Tenant",
                    Identifier = "default-tenant"
                };
                context.Tenants.Add(defaultTenant);
                await context.SaveChangesAsync();
            }

            // Step 2: Seed default roles (optional)
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
                await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            // Step 3: Seed SuperAdmin user
            var superAdminEmail = "admin@default.com";
            var superAdmin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == superAdminEmail);

            if (superAdmin == null)
            {
                var user = new User
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    TenantId = defaultTenant.Id
                };

                var result = await userManager.CreateAsync(user, "Admin@123"); // Use a strong password
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }
    }
}
