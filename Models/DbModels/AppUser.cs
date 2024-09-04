using Microsoft.AspNetCore.Identity;
namespace RecyclingHub.Models.DbModels
{
    public class AppUser:IdentityUser
    {
        public string CompanyName {  get; set; } = string.Empty;
    }
}
