using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models
{
	public class PasswordChangeModel
	{
		[Required]
		public string OldPassword { get; set; }

		[Required]
		public string NewPassword { get; set; }
	}
}
