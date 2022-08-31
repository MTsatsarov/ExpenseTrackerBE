
namespace ExpenseTracker.Data.Common
{
	public interface IDeletableEntity
	{
		bool IsDeleted{ get; set; }

		DateTime? DeletedOn { get; set; }
	}
}
