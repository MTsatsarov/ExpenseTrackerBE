namespace ExpenseTracker.Services.Models.Client
{
	public class ClientList
	{
		public string Id { get; set; }

		public string Email { get; set; }

		public string FirstName{ get; set; }

		public string LastName { get; set; }

		public string Organization { get; set; }

		public int TotalTransactions { get; set; }

		public string CreatedOn { get; set; }
	}
}
