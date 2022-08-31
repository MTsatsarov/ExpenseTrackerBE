using ExpenseTracker.Data.Common;

namespace ExpenseTracker.Data.Entities
{
	public class ExpenseProducts : BaseDeletableEntity<Guid>
	{
		public ExpenseProducts()
		{
			this.Id = Guid.NewGuid();
		}

		public decimal Price { get; set; }

		public decimal Quantity { get; set; }

		public Guid ProductId{ get; set; }

		public virtual Product Product { get; set; }

		public Guid ExpenseId { get; set; }

		public virtual Expense Expense { get; set; }
	}
}