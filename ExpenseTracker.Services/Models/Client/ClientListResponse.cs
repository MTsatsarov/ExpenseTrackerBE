namespace ExpenseTracker.Services.Models.Client
{
	public class ClientListResponse
	{
		public int Count => Clients.Count();

		public IEnumerable<ClientList> Clients{ get; set; }
	}
}
