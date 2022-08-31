using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models
{
	public class UserRegisterModel
	{
		[Required]
		[StringLength(35,MinimumLength =2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(35, MinimumLength = 2)]
		public string LastName { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Email{ get; set; }

	}
}
