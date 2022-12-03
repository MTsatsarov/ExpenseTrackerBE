namespace ExpenseTracker.Services.Models.User
{
	public abstract class BaseUserInfo
	{
		public string UserId { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string CurrencySymbol { get; set; }
	}
}
