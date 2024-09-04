using System.ComponentModel.DataAnnotations;
namespace RecyclingHub.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is Required")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = string.Empty;
    }
}
