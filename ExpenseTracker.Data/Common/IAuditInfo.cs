
namespace ExpenseTracker.Data.Common
{
	public interface IAuditInfo
	{
		 DateTime CreatedOn { get; set; }
		 DateTime? ModifiedOn { get; set; }
	}
}
