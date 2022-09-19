
using ExpenseTracker.Data.Entities;

namespace ExpenseTracker.Services.Models.User
{
	public class UserResponse
	{
		public UserResponse(ApplicationUser user)
		{
			UserId = user.Id;
			Email = user.Email;
			FirstName = user.FirstName;
			LastName = user.LastName;
		}
		public string UserId { get; set; }
		
		public string Role { get; set; }

		public string Email { get; set; }

		public string FirstName{ get; set; }

		public string LastName { get; set; }
	}
}
