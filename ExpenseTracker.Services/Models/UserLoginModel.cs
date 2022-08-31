
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models
{
	public class UserLoginModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
