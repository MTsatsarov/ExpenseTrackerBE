namespace ExpenseTracker.Services.Models.Organization
{
	public class OrganizationUserResponse
	{
		public IEnumerable<OrganizationUserList> Employees { get; set; }

		public int Count { get; set; }
	}
}
