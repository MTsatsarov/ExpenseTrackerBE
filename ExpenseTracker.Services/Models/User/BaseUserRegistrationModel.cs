
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Models.User
{
	public abstract class BaseUserRegistrationModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[StringLength(35, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(35, MinimumLength = 2)]
		public string LastName { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Email { get; set; }
	}
}
