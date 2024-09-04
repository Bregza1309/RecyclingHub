using System.ComponentModel.DataAnnotations; 
namespace RecyclingHub.Models.ViewModels
{
    public class EmployeeViewModel
    {
        [Required(ErrorMessage ="Username is Required")]
        public string Username { get; set; } = string.Empty;
        
        public string CompanyName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please confirm the Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
