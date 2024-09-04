using RecyclingHub.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RecyclingHub.Data
{
    public class IdentityContext(DbContextOptions<IdentityContext> opts):IdentityDbContext<AppUser>(opts)
    {
        
    }
}
