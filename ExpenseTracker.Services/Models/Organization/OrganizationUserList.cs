namespace ExpenseTracker.Services.Models.Organization
{
	public class OrganizationUserList
	{
		public string Id { get; set; }
		public string Email { get; set; }

		public string UserName { get; set; }

		public string CreatedOn { get; set; }

		public int TotalTransactions { get; set; }

		public  decimal HighestSum { get; set; }

		public decimal TotalSum { get; set; }

		public string LastTransaction { get; set; }
	}
}
