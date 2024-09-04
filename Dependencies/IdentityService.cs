using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecyclingHub.Models.DbModels;
namespace RecyclingHub.Dependencies
{
    public class IdentityService(UserManager<AppUser> _userManager)
    {
        readonly UserManager<AppUser> UserManager = _userManager;

        public  async Task<List<AppUser>> GetEmployees(string companyName , string adminName)
        {
            var employees = await UserManager.Users
                .Where(u => u.CompanyName == companyName && u.UserName != adminName).ToListAsync();
            
            return employees;
        }
        public  string GetCompanyNameAsync(string username)
        {
            var user = UserManager.FindByNameAsync(username).Result;
            if(user != null)
            {
                return user.CompanyName;
            }
            return string.Empty;
        }
        public async Task<AppUser?> GetAsync(string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);
            return user;
        }
        public async Task DeleteAsync(string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);
            if(user != null)
            {
                await UserManager.DeleteAsync(user);
            }
        }
        public string GetRoleByName(string userName)
        {
            var user = UserManager.FindByNameAsync(userName).Result;
            var roles = UserManager.GetRolesAsync(user).Result;
            return roles.First();
        }
        public string GetIdbyName(string userName)
        {
            var user = UserManager.FindByNameAsync(userName).Result;
            return user!.Id;
        }
    }
}
