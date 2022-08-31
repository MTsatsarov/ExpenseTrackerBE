namespace ExpenseTracker.Services.Models
{
	public class RefreshRequest
	{
		public string UserId { get; set; }

		public string RefreshToken { get; set; }
	}
}
