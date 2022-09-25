using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.User
{
	public class UserUpdateModel
	{
		public string Id { get; set; }

		public string UserName { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}
