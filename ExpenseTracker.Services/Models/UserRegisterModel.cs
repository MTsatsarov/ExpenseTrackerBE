using ExpenseTracker.Services.Models.User;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models
{
	public class UserRegisterModel : BaseUserRegistrationModel
	{
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Organization { get; set; }
	}
}
