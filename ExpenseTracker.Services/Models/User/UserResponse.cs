
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
			CurrencySymbol = user.Organization.CurrencySymbol;
		}
		public string UserId { get; set; }
		
		public IEnumerable<string> Roles { get; set; }

		public string Email { get; set; }

		public string FirstName{ get; set; }

		public string LastName { get; set; }

		public string CurrencySymbol { get; set; }
	}
}
