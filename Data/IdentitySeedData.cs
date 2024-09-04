using RecyclingHub.Models.DbModels;
using Microsoft.AspNetCore.Identity;
namespace RecyclingHub.Data
{
    public static class IdentitySeedData
    {
        public static async Task EnsureIdentitySeeded(IApplicationBuilder app)
        {

            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<AppUser> userManager = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            if (!roleManager.Roles.Any())
            {
                string[] rolesArray = { "Admin" , "Employee"};
                var roles = rolesArray.Select(x => new IdentityRole { Name = x}).ToList();
                foreach(var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
            if (!userManager.Users.Any())
            {
                //add default admin
                AppUser admin = new()
                {
                    UserName = "Stanley",
                    CompanyName = "Stanlo Bro`s"
                };
                await userManager.CreateAsync(admin,"Pa$$w0rd");
                await userManager.AddToRoleAsync(admin,"Admin");
                //add default admin
                admin = new()
                {
                    UserName = "Bregza",
                    CompanyName = "Bregza's Scrap"
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

        }
    }
}
