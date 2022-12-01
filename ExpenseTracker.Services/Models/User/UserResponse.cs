
using ExpenseTracker.Data.Entities;

namespace ExpenseTracker.Services.Models.User
{
	public class UserResponse:BaseUserInfo
	{
		public UserResponse(ApplicationUser user)
		{
			this.UserId = user.Id;
			this.Email = user.Email;
			this.FirstName = user.FirstName;
			this.LastName = user.LastName;
			this.CurrencySymbol = user.Organization.CurrencySymbol;
			this.Mode = user.Settings.Mode;
		}
		
		public IEnumerable<string> Roles { get; set; }

		public string Mode { get; set; }
	}
}
