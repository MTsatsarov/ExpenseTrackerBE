
using ExpenseTracker.Data.Entities;
using System.Globalization;

namespace ExpenseTracker.Services.Models.User
{
	public class UserInfo:BaseUserInfo
	{
		public UserInfo(ApplicationUser user)
		{
			this.UserId = user.Id;
			this.UserName = user.UserName;
			this.Email = user.Email;
			this.FirstName = user.FirstName;
			this.PhoneNumber = user.PhoneNumber;
			this.LastName = user.LastName;
			this.CurrencySymbol = user.Organization.CurrencySymbol;
			this.Organization = user.Organization.Name;
			this.EmailConfirmed = user.EmailConfirmed;

			this.RegistrationDate = user
				.CreatedOn
				.ToString("MM/dd/yyyy",CultureInfo.InvariantCulture);
		}
		public string PhoneNumber { get; set; }

		public string Organization { get; set; }

		public string RegistrationDate { get; set; }
		public bool EmailConfirmed { get; set; }
	}
}
