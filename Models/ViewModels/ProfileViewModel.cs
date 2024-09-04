using System.ComponentModel.DataAnnotations;

namespace RecyclingHub.Models.ViewModels
{
	public class ProfileViewModel
	{
		public string Id { get; set; } = string.Empty;
		public string? Username { get; set; } 
		
		public string? Password { get; set; }
		
		public string? ConfirmPassword { get; set; }
	}
}
